using System;
using System.Collections.Generic;
using Core;
using UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Tower
{
    public class TowerManager : Singleton.Singleton<TowerManager>
    {
        [SerializeField] private int shootingTowerCost;
        [SerializeField] private int slowingTowerCost;
        [SerializeField] private float towerSpawnYOffset;
        [SerializeField] private LayerMask pathColliderLayer;

        [Header("References")]
        [SerializeField] private List<BaseTower> towersToSpawn;

        [Header("Views")]
        [SerializeField] private TowerShopView towerShopView;
        [SerializeField] private SelectedTowerMenuView selectedTowerMenuView;
        [SerializeField] private TowerManagementPanel towerManagementPanel;

        [FormerlySerializedAs("rangeTowerCreator")]
        [Header("Factories")]
        [SerializeField] private RangeBaseTowerFactory rangeBaseTowerFactory;
        [FormerlySerializedAs("slowingTowerCreator")]
        [SerializeField] private SlowingBaseTowerFactory slowingBaseTowerFactory;

        [FormerlySerializedAs("_projectilePrefab")]
        [Header("Projectile")]
        [SerializeField] private Projectile.Projectile projectilePrefab;

        private BaseTower selectedTower;
        private BaseTower selectedTowerComponent;
        private readonly List<BaseTower> placedTowers = new List<BaseTower>();

        // Słownik do mapowania instancji wieży na jej prefab
        private readonly Dictionary<BaseTower, BaseTower> towerInstanceToPrefab = new();

        public ObjectPool<Projectile.Projectile> projectilePool;

        protected override void Awake()
        {
            SetTowerCosts();

            projectilePool = new ObjectPool<Projectile.Projectile>(
                createFunc: () =>
                {
                    Projectile.Projectile instance = Instantiate(projectilePrefab);
                    instance.Pool = projectilePool;
                    instance.gameObject.SetActive(false);
                    return instance;
                },
                actionOnGet: projectile =>
                {
                    projectile.gameObject.SetActive(true);
                    projectile.ResetState();
                },
                actionOnRelease: projectile => projectile.gameObject.SetActive(false),
                actionOnDestroy: projectile => Destroy(projectile.gameObject),
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 100
            );
        }

        // Metoda do postawienia wieży (bez zwracania instancji)
        public void PlaceTower(BaseTower towerPrefab, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(towerPrefab.GetCost());
            
            GameObject towerTempObject = new GameObject("TempTowerTransform");
            towerTempObject.transform.position = position + new Vector3(0, towerSpawnYOffset, 0);
            towerTempObject.transform.rotation = Quaternion.identity;

            BaseTower newTowerInstance = null;
            if (towerPrefab is RangeTower)
            {
                newTowerInstance = rangeBaseTowerFactory.CreateTower(towerTempObject.transform);
            }
            else if (towerPrefab is SlowingTower)
            {
                newTowerInstance = slowingBaseTowerFactory.CreateTower(towerTempObject.transform);
            }

            if (newTowerInstance != null)
            {
                placedTowers.Add(newTowerInstance);
                towerInstanceToPrefab[newTowerInstance] = towerPrefab;

                TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.PlaceTowerTutorialName, 2);
            }
            else
            {
                Debug.LogError("Nie udało się stworzyć wieży.");
            }
        }

        // Metoda do postawienia wieży, która zwraca instancję (do komend)
        public BaseTower PlaceTowerAndReturn(BaseTower towerPrefab, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(towerPrefab.GetCost());
    
            GameObject towerTempObject = new GameObject("TempTowerTransform");
            towerTempObject.transform.position = position + new Vector3(0, towerSpawnYOffset, 0);
            towerTempObject.transform.rotation = Quaternion.identity;

            BaseTower newTowerInstance = null;
            if (towerPrefab is RangeTower)
            {
                newTowerInstance = rangeBaseTowerFactory.CreateTower(towerTempObject.transform);
            }
            else if (towerPrefab is SlowingTower)
            {
                newTowerInstance = slowingBaseTowerFactory.CreateTower(towerTempObject.transform);
            }

            if (newTowerInstance != null)
            {
                // 🔑 Kluczowe informacje do cofania
                newTowerInstance.originalPrefab = towerPrefab.gameObject;

                // 💾 Zarejestruj instancję z prefabem
                RegisterTower(newTowerInstance, towerPrefab);

                placedTowers.Add(newTowerInstance);

                TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.PlaceTowerTutorialName, 2);
                return newTowerInstance;
            }

            Debug.LogError("Nie udało się stworzyć wieży.");
            return null;
        }



        // Zwraca prefab dla danej instancji wieży
        public BaseTower GetPrefabForTower(BaseTower towerInstance)
        {
            if (towerInstanceToPrefab.TryGetValue(towerInstance, out BaseTower prefab))
            {
                return prefab;
            }
            Debug.LogWarning("Nie znaleziono prefab dla podanej wieży.");
            return null;
        }

        // Zwraca pieniądze za wieżę i usuwa ją z list/słownika
        public void RefundTower(BaseTower tower)
        {
            if (tower == null) return;

            int refundAmount = Mathf.RoundToInt(tower.GetCost());
            Core.LevelManager.Instance.AddMoney(refundAmount);

            if (placedTowers.Contains(tower))
                placedTowers.Remove(tower);

            if (towerInstanceToPrefab.ContainsKey(tower))
                towerInstanceToPrefab.Remove(tower);
        }
        
        public void RegisterTower(BaseTower instance, BaseTower prefab)
        {
            towerInstanceToPrefab[instance] = prefab;
        }

        public void SellTower()
        {
            if (!selectedTower) return;
            if (selectedTowerComponent == null) return;

            Core.LevelManager.Instance.AddMoney((int)(selectedTowerComponent.GetCost() *
                                                      selectedTowerComponent.GetSellModifier()));

            DestroySelectedTower();
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 2);
        }

        public bool CanPlaceTower(BaseTower tower, Vector3 position)
        {
            if (!tower)
                return false;

            CapsuleCollider towerCollider = tower.GetComponentInChildren<CapsuleCollider>();

            if (!towerCollider)
                return false;

            if (Physics.CheckSphere(position, towerCollider.radius, pathColliderLayer))
                return false;

            int cost = tower.GetCost();
            return Core.LevelManager.Instance.EnoughMoney(cost);
        }

        public void SelectTower(BaseTower selectedTower)
        {
            if (selectedTower == null)
            {
                Debug.LogError("Selected tower is null!");
                return;
            }

            this.selectedTower = selectedTower;
            selectedTowerComponent = selectedTower.GetComponent<BaseTower>();

            if (selectedTowerComponent == null)
            {
                Debug.LogError($"Selected GameObject '{selectedTower.name}' does not have a BaseTower component!");
                return;
            }

            if (selectedTowerMenuView != null)
            {
                selectedTowerMenuView.SetViewActive(true);
                selectedTowerMenuView.SetTowerName(selectedTowerComponent.GetTowerName());
            }
            else
            {
                Debug.LogError("selectedTowerMenuView is not assigned in the inspector!");
            }

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 1);
        }

        public void UnselectTower()
        {
            this.selectedTower = null;
            selectedTowerMenuView.SetViewActive(false);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 0);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.EnemyAttackTutorialName, 1);
        }

        private void Update()
        {
            if (!Core.LevelManager.Instance.EnoughMoney(shootingTowerCost))
            {
                towerShopView.ShootingTowerButtonInteractable(false);
            }

            if (!Core.LevelManager.Instance.EnoughMoney(slowingTowerCost))
            {
                towerShopView.SlowingTowerButtonInteractable(false);
            }

            if (Core.LevelManager.Instance.EnoughMoney(shootingTowerCost))
            {
                towerShopView.ShootingTowerButtonInteractable(true);
            }

            if (Core.LevelManager.Instance.EnoughMoney(slowingTowerCost))
            {
                towerShopView.SlowingTowerButtonInteractable(true);
            }
        }

        private void SetTowerCosts()
        {
            RangeTower.cost = shootingTowerCost;
            SlowingTower.cost = slowingTowerCost;
            towerShopView.UpdateShootingTowerCost(shootingTowerCost);
            towerShopView.UpdateSlowingTowerCost(slowingTowerCost);
        }

        private void DestroySelectedTower()
        {
            selectedTowerMenuView.SetViewActive(false);
            towerManagementPanel.ClosePanel();
            placedTowers.Remove(selectedTower);
            if (towerInstanceToPrefab.ContainsKey(selectedTower))
                towerInstanceToPrefab.Remove(selectedTower);
            Destroy(selectedTower.gameObject);
            selectedTower = null;
        }
    }
}

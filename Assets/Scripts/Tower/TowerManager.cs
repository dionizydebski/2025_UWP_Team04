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

        [Header("Factories")]
        [SerializeField] private RangeBaseTowerFactory rangeBaseTowerFactory;
        [SerializeField] private SlowingBaseTowerFactory slowingBaseTowerFactory;

        [Header("Projectile")]
        [SerializeField] private Projectile.Projectile projectilePrefab;

        public BaseTower selectedTower;
        private BaseTower selectedTowerComponent;
        private readonly List<BaseTower> placedTowers = new();
        private readonly Dictionary<BaseTower, BaseTower> towerInstanceToPrefab = new();

        public ObjectPool<Projectile.Projectile> projectilePool;

        protected override void Awake()
        {
            SetTowerCosts();

            projectilePool = new ObjectPool<Projectile.Projectile>(
                createFunc: () =>
                {
                    var instance = Instantiate(projectilePrefab);
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
                newTowerInstance.originalPrefab = towerPrefab.gameObject;
                RegisterTower(newTowerInstance, towerPrefab);
                placedTowers.Add(newTowerInstance);

                TutorialEventsManager.Instance.TriggerTutorialStepEvent(
                    TutorialEventsManager.PlaceTowerTutorialName, 2
                );

                return newTowerInstance;
            }

            Debug.LogError("Nie udało się stworzyć wieży.");
            return null;
        }
        
        public BaseTower PlaceTowerAndReturnWithRefundCost(BaseTower towerPrefab, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney((int)(towerPrefab.GetCost()*0.7));

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
                newTowerInstance.originalPrefab = towerPrefab.gameObject;
                RegisterTower(newTowerInstance, towerPrefab);
                placedTowers.Add(newTowerInstance);

                TutorialEventsManager.Instance.TriggerTutorialStepEvent(
                    TutorialEventsManager.PlaceTowerTutorialName, 2
                );

                return newTowerInstance;
            }

            Debug.LogError("Nie udało się stworzyć wieży.");
            return null;
        }

        public void RemoveTower(BaseTower tower)
        {
            if (tower == null) return;

            if (placedTowers.Contains(tower))
                placedTowers.Remove(tower);

            if (towerInstanceToPrefab.ContainsKey(tower))
                towerInstanceToPrefab.Remove(tower);

            Destroy(tower.gameObject);
        }

        public void RefundTower(BaseTower tower)
        {
            if (tower == null) return;

            int refundAmount = tower.GetCost();
            Core.LevelManager.Instance.AddMoney(refundAmount);

            placedTowers.Remove(tower);
            towerInstanceToPrefab.Remove(tower);
            
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 2);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.UpgradeTowerTutorialName, 0);
        }
        
        public void SellTower(BaseTower tower)
        {
            if (tower == null) return;

            int refundAmount = (int)(tower.GetCost() * 0.7);
            Core.LevelManager.Instance.AddMoney(refundAmount);

            placedTowers.Remove(tower);
            towerInstanceToPrefab.Remove(tower);
            
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 2);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.UpgradeTowerTutorialName, 0);
        }

        public BaseTower GetPrefabForTower(BaseTower towerInstance)
        {
            if (towerInstanceToPrefab.TryGetValue(towerInstance, out BaseTower prefab))
            {
                return prefab;
            }
            Debug.LogWarning("Nie znaleziono prefab dla podanej wieży.");
            return null;
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

            return Core.LevelManager.Instance.EnoughMoney(tower.GetCost());
        }

        public void SelectTower(BaseTower tower)
        {
            if (!tower)
            {
                Debug.LogError("Selected tower is null!");
                return;
            }

            selectedTower = tower;
            selectedTowerComponent = tower.GetComponent<BaseTower>();

            if (!selectedTowerComponent)
            {
                Debug.LogError($"Selected GameObject '{tower.name}' nie ma komponentu BaseTower!");
                return;
            }

            if (selectedTowerMenuView != null)
            {
                selectedTowerMenuView.SetViewActive(true);
                selectedTowerMenuView.SetTowerName(selectedTowerComponent.GetTowerName());
                
                TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.UpgradeTowerTutorialName, 1);
            }
            else
            {
                Debug.LogError("Brakuje referencji do selectedTowerMenuView!");
            }

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 1);
        }

        public void UnselectTower()
        {
            selectedTower = null;
            selectedTowerComponent = null;

            selectedTowerMenuView.SetViewActive(false);
            towerManagementPanel.ClosePanel();

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 0);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.EnemyAttackTutorialName, 1);
        }

        public void RegisterTower(BaseTower instance, BaseTower prefab)
        {
            if (!towerInstanceToPrefab.ContainsKey(instance))
            {
                towerInstanceToPrefab[instance] = prefab;
            }
        }

        private void Update()
        {
            bool enoughMoneyShooting = Core.LevelManager.Instance.EnoughMoney(shootingTowerCost);
            bool enoughMoneySlowing = Core.LevelManager.Instance.EnoughMoney(slowingTowerCost);

            towerShopView.ShootingTowerButtonInteractable(enoughMoneyShooting);
            towerShopView.SlowingTowerButtonInteractable(enoughMoneySlowing);
        }

        private void SetTowerCosts()
        {
            RangeTower.cost = shootingTowerCost;
            SlowingTower.cost = slowingTowerCost;

            towerShopView.UpdateShootingTowerCost(shootingTowerCost);
            towerShopView.UpdateSlowingTowerCost(slowingTowerCost);
        }
    }
}

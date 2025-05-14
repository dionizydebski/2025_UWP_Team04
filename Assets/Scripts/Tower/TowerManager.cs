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
        [FormerlySerializedAs("slowingTowerCreator")] [SerializeField] private SlowingBaseTowerFactory slowingBaseTowerFactory;
        
        [FormerlySerializedAs("_projectilePrefab")]
        [Header("Projectile")]
        [SerializeField] private Projectile.Projectile projectilePrefab;

        private BaseTower selectedTower;
        private BaseTower selectedTowerComponent;
        private readonly List<BaseTower> placedTowers = new List<BaseTower>();
        
        public ObjectPool<Projectile.Projectile> projectilePool;
        
        protected override void Awake()
        {
            SetTowerCosts();
            
            projectilePool = new ObjectPool<Projectile.Projectile>(
                createFunc: () => {
                    Projectile.Projectile instance = Instantiate(projectilePrefab);
                    instance.Pool = projectilePool;
                    instance.gameObject.SetActive(false);
                    return instance;
                }, 
                actionOnGet: projectile => {
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
        
        public void PlaceTower(BaseTower tower, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(tower.GetCost());
            GameObject towerTempObject = new GameObject("TempTowerTransform");
            towerTempObject.transform.position = position + new Vector3(0, towerSpawnYOffset, 0);
            towerTempObject.transform.rotation = Quaternion.identity;
            if (tower is RangeTower)
            {
                //Debug.Log("Range tower place");
                rangeBaseTowerFactory.CreateTower(towerTempObject.transform);
            }
            else if (tower is SlowingTower)
            {
                Debug.Log("Range tower place");
                slowingBaseTowerFactory.CreateTower(towerTempObject.transform);
            }
            placedTowers.Add(tower);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.PlaceTowerTutorialName, 2);
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
            Destroy(selectedTower.gameObject);
            selectedTower = null;
        }


    }
}
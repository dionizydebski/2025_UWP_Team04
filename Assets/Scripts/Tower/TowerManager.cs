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

        public void PlaceTower(BaseTower towerPrefab, Vector3 position)
        {
            PlaceTowerAndReturn(towerPrefab, position);
        }

        public BaseTower PlaceTowerAndReturn(BaseTower towerPrefab, Vector3 position)
        {
            if (!CanPlaceTower(towerPrefab, position))
            {
                Debug.LogWarning("Cannot place tower at: " + position);
                return null;
            }

            LevelManager.Instance.SpendMoney(towerPrefab.GetCost());

            GameObject towerTempObject = new GameObject("TempTowerTransform");
            towerTempObject.transform.position = position + new Vector3(0, towerSpawnYOffset, 0);
            towerTempObject.transform.rotation = Quaternion.identity;

            BaseTower createdTower = null;

            if (towerPrefab is RangeTower)
            {
                createdTower = rangeBaseTowerFactory.CreateTower(towerTempObject.transform);
            }
            else if (towerPrefab is SlowingTower)
            {
                createdTower = slowingBaseTowerFactory.CreateTower(towerTempObject.transform);
            }

            if (createdTower != null)
            {
                placedTowers.Add(createdTower);
            }

            Destroy(towerTempObject);
            return createdTower;
        }

        public void RefundTower(BaseTower tower)
        {
            if (tower == null) return;

            int refund = Mathf.RoundToInt(tower.GetCost() * tower.GetSellModifier());
            LevelManager.Instance.AddMoney(refund);
        }

        public void SellTower()
        {
            if (!selectedTower || selectedTowerComponent == null) return;

            RefundTower(selectedTowerComponent);
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
            {
                Debug.LogWarning("Blocked by path layer at position: " + position);
                return false;
            }

            int cost = tower.GetCost();
            return LevelManager.Instance.EnoughMoney(cost);
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

            selectedTowerMenuView?.SetViewActive(true);
            selectedTowerMenuView?.SetTowerName(selectedTowerComponent.GetTowerName());

            towerManagementPanel?.OpenPanel(selectedTowerComponent);

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 1);
        }

        public void UnselectTower()
        {
            selectedTower = null;
            selectedTowerComponent = null;

            selectedTowerMenuView?.SetViewActive(false);
            towerManagementPanel?.ClosePanel();

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 0);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.EnemyAttackTutorialName, 1);
        }

        private void DestroySelectedTower()
        {
            selectedTowerMenuView?.SetViewActive(false);
            towerManagementPanel?.ClosePanel();

            if (selectedTower != null)
            {
                placedTowers.Remove(selectedTower);
                Destroy(selectedTower.gameObject);
                selectedTower = null;
                selectedTowerComponent = null;
            }
        }

        private void Update()
        {
            bool canAffordShooting = LevelManager.Instance.EnoughMoney(shootingTowerCost);
            bool canAffordSlowing = LevelManager.Instance.EnoughMoney(slowingTowerCost);

            towerShopView.ShootingTowerButtonInteractable(canAffordShooting);
            towerShopView.SlowingTowerButtonInteractable(canAffordSlowing);
        }

        private void SetTowerCosts()
        {
            RangeTower.cost = shootingTowerCost;
            SlowingTower.cost = slowingTowerCost;

            towerShopView.UpdateShootingTowerCost(shootingTowerCost);
            towerShopView.UpdateSlowingTowerCost(slowingTowerCost);
        }
        
        public BaseTower GetPrefabForTower(BaseTower tower)
        {
            if (tower is RangeTower)
                return towersToSpawn.Find(t => t is RangeTower);
            if (tower is SlowingTower)
                return towersToSpawn.Find(t => t is SlowingTower);
            return null;
        }

    }
}

using System.Collections.Generic;
using Core;
using Core.Commands;
using UI;
using UnityEngine;
using UnityEngine.Pool;

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
        private readonly List<BaseTower> placedTowers = new();

        public ObjectPool<Projectile.Projectile> projectilePool;
        public CommandManager commandManager = new CommandManager();

        protected override void Awake()
        {
            SetTowerCosts();

            projectilePool = new ObjectPool<Projectile.Projectile>(
                () => {
                    var instance = Instantiate(projectilePrefab);
                    instance.Pool = projectilePool;
                    instance.gameObject.SetActive(false);
                    return instance;
                },
                projectile => {
                    projectile.gameObject.SetActive(true);
                    projectile.ResetState();
                },
                projectile => projectile.gameObject.SetActive(false),
                projectile => Destroy(projectile.gameObject),
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 100
            );
        }

        public void PlaceTower(BaseTower tower, Vector3 position)
        {
            if (!CanPlaceTower(tower, position)) return;

            var placeCommand = new PlaceTowerCommand(tower, position);
            commandManager.ExecuteCommand(placeCommand);
        }

        public void SellTower()
        {
            if (selectedTowerComponent == null) return;

            var sellCommand = new SellTowerCommand(selectedTowerComponent);
            commandManager.ExecuteCommand(sellCommand);

            UnselectTower();
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 2);
        }

        public void RefundTower(BaseTower tower)
        {
            int refundAmount = (int)(tower.GetCost() * tower.GetSellModifier());
            Core.LevelManager.Instance.AddMoney(refundAmount);
        }

        public bool CanPlaceTower(BaseTower tower, Vector3 position)
        {
            if (!tower) return false;

            var towerCollider = tower.GetComponentInChildren<CapsuleCollider>();
            if (!towerCollider) return false;

            if (Physics.CheckSphere(position, towerCollider.radius, pathColliderLayer)) return false;

            return Core.LevelManager.Instance.EnoughMoney(tower.GetCost());
        }

        public void SelectTower(BaseTower tower)
        {
            if (tower == null)
            {
                Debug.LogError("Selected tower is null!");
                return;
            }

            selectedTower = tower;
            selectedTowerComponent = tower.GetComponent<BaseTower>();

            if (selectedTowerComponent == null)
            {
                Debug.LogError($"'{tower.name}' has no BaseTower component!");
                return;
            }

            selectedTowerMenuView?.SetViewActive(true);
            selectedTowerMenuView?.SetTowerName(selectedTowerComponent.GetTowerName());

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 1);
        }

        public void UnselectTower()
        {
            selectedTower = null;
            selectedTowerComponent = null;
            selectedTowerMenuView?.SetViewActive(false);

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SellTowerTutorialName, 0);
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.EnemyAttackTutorialName, 1);
        }

        private void Update()
        {
            towerShopView.ShootingTowerButtonInteractable(Core.LevelManager.Instance.EnoughMoney(shootingTowerCost));
            towerShopView.SlowingTowerButtonInteractable(Core.LevelManager.Instance.EnoughMoney(slowingTowerCost));
        }

        public BaseTower PlaceTowerAndReturn(BaseTower towerPrefab, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(towerPrefab.GetCost());

            var tempObj = new GameObject("TempTowerTransform");
            tempObj.transform.position = position + new Vector3(0, towerSpawnYOffset, 0);
            tempObj.transform.rotation = Quaternion.identity;

            BaseTower createdTower = null;

            if (towerPrefab is RangeTower)
                createdTower = rangeBaseTowerFactory.CreateTower(tempObj.transform);
            else if (towerPrefab is SlowingTower)
                createdTower = slowingBaseTowerFactory.CreateTower(tempObj.transform);

            if (createdTower != null)
                placedTowers.Add(createdTower);

            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.PlaceTowerTutorialName, 2);
            return createdTower;
        }

        public void RemoveTower(BaseTower tower)
        {
            placedTowers.Remove(tower);
            Destroy(tower.gameObject);
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

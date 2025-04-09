using System.Collections.Generic;
using Singleton;
using UI;
using UnityEngine;

namespace Tower
{
    public class TowerManager : Singleton<TowerManager>
    {
        [SerializeField] private int shootingTowerCost;
        [SerializeField] private int slowingTowerCost;
        [SerializeField] private float towerSpawnYOffset;
        [SerializeField] private LayerMask pathColliderLayer;

        [Header("References")] [SerializeField]
        private List<BaseTower> towersToSpawn;

        [Header("Views")] [SerializeField] private TowerShopView towerShopView;
        [SerializeField] private SelectedTowerMenuView selectedTowerMenuView;

        private GameObject selectedTower;
        private BaseTower selectedTowerComponent;

        public void PlaceTower(GameObject tower, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(tower.GetComponent<BaseTower>().GetCost());
            Instantiate(tower, position + new Vector3(0, towerSpawnYOffset, 0), Quaternion.identity);
        }

        public void SellTower()
        {
            if (!selectedTower) return;

            if (selectedTowerComponent == null) return;

            Core.LevelManager.Instance.AddMoney((int)(selectedTowerComponent.GetCost() *
                                                      selectedTowerComponent.GetSellModifier()));

            DestroySelectedTower();
        }

        public bool CanPlaceTower(GameObject tower, Vector3 position)
        {
            if (!tower)
                return false;

            BaseTower towerComponent = tower.GetComponent<BaseTower>();
            CapsuleCollider towerCollider = tower.GetComponentInChildren<CapsuleCollider>();

            if (!towerComponent || !towerCollider)
                return false;

            if (Physics.CheckSphere(position, towerCollider.radius, pathColliderLayer))
                return false;

            int cost = towerComponent.GetCost();
            return Core.LevelManager.Instance.EnoughMoney(cost);
        }

        public void SelectTower(GameObject selectedTower)
        {
            this.selectedTower = selectedTower;
            selectedTowerComponent = selectedTower.GetComponent<BaseTower>();
            selectedTowerMenuView.SetViewActive(true);
            selectedTowerMenuView.SetTowerName(selectedTowerComponent.GetTowerName());
        }

        public void UnselectTower()
        {
            this.selectedTower = null;
            selectedTowerMenuView.SetViewActive(false);
        }


        protected override void Awake()
        {
            SetTowerCosts();
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
            ShootingTower.cost = shootingTowerCost;
            SlowingTower.cost = slowingTowerCost;
            towerShopView.UpdateShootingTowerCost(shootingTowerCost);
            towerShopView.UpdateSlowingTowerCost(slowingTowerCost);
        }

        private void DestroySelectedTower()
        {
            selectedTowerMenuView.SetViewActive(false);
            Destroy(selectedTower);
            selectedTower = null;
        }
    }
}
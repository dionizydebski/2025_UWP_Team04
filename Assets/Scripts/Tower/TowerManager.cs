using System;
using System.Collections.Generic;
using Core;
using UI;
using UnityEngine;

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

        private GameObject selectedTower;
        private BaseTower selectedTowerComponent;
        private readonly List<GameObject> placedTowers = new List<GameObject>();
        
        protected override void Awake()
        {
            SetTowerCosts();
        }
        
        public void PlaceTower(GameObject tower, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(tower.GetComponent<BaseTower>().GetCost());
            Instantiate(tower, position + new Vector3(0, towerSpawnYOffset, 0), Quaternion.identity);
            placedTowers.Add(tower);
            TutorialEventsManager.Instance.TriggerTutorialEvent(TutorialEventsManager.PlaceTowerTutorialName, 2);
        }

        public void SellTower()
        {
            if (!selectedTower) return;

            if (selectedTowerComponent == null) return;

            Core.LevelManager.Instance.AddMoney((int)(selectedTowerComponent.GetCost() *
                                                      selectedTowerComponent.GetSellModifier()));

            DestroySelectedTower();
            TutorialEventsManager.Instance.TriggerTutorialEvent(TutorialEventsManager.SellTowerTutorialName, 2);
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

            TutorialEventsManager.Instance.TriggerTutorialEvent(TutorialEventsManager.SellTowerTutorialName, 1);
        }

        public void UnselectTower()
        {
            this.selectedTower = null;
            selectedTowerMenuView.SetViewActive(false);
            TutorialEventsManager.Instance.TriggerTutorialEvent(TutorialEventsManager.SellTowerTutorialName, 0);
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
            towerManagementPanel.ClosePanel();
            placedTowers.Remove(selectedTower);
            Destroy(selectedTower);
            selectedTower = null;
        }


    }
}
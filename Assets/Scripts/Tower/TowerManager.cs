using System.Collections.Generic;
using Singleton;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tower
{
    public class TowerManager : Singleton<TowerManager>
    {
        private List<BaseTower> towers = new List<BaseTower>();
        
        [SerializeField] private int shootingTowerCost;
        [SerializeField] private int slowingTowerCost;
        [SerializeField] private TowerMenuController towerMenuController;
        [SerializeField] private float towerSpawnYOffset;
        [SerializeField] private LayerMask pathColliderLayer;
        
        [Header("References")]
        [SerializeField] private List<BaseTower> towersToSpawn;

        protected override void Awake()
        {
            SetTowerCosts();
        }

        private void SetTowerCosts()
        {
            ShootingTower.cost = shootingTowerCost;
            SlowingTower.cost = slowingTowerCost;
            towerMenuController.UpdateShootingTowerCost(shootingTowerCost);
            towerMenuController.UpdateSlowingTowerCost(slowingTowerCost);
        }

        public void PlaceTower(GameObject tower, Vector3 position)
        {
            Core.LevelManager.Instance.SpendMoney(tower.GetComponent<BaseTower>().GetCost());
            Instantiate(tower, position + new Vector3(0,towerSpawnYOffset, 0), Quaternion.identity);
        }

        public void SellTower(BaseTower tower)
        {
            Core.LevelManager.Instance.AddMoney((int)(tower.GetCost() * 0.7));
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

        public void SelectTower()
        {
            
        }
    }
}
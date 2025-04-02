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

        private void Awake()
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

        public void PlaceTower(BaseTower tower)
        {
            
        }

        public void SellTower(BaseTower tower)
        {
            
        }

        public bool CanPlaceTower(Vector3 position)
        {
            return true;
        }
    }
}
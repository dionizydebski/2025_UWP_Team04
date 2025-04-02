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
        
        [Header("References")]
        [SerializeField] private List<BaseTower> towersToSpawn;

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

        public void PlaceTower(GameObject tower, Vector3 position)
        {
            Instantiate(tower, position, Quaternion.identity);
        }

        public void SellTower(BaseTower tower)
        {
            
        }

        private bool CanPlaceTower(Vector3 position)
        {
            return true;
        }
    }
}
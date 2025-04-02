using System.Collections.Generic;
using Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tower
{
    public class TowerManager : Singleton<TowerManager>
    {
        private List<BaseTower> towers = new List<BaseTower>();
        
        [SerializeField] private TMP_Text shootingTowerCostText; 
        [SerializeField] private TMP_Text slowingTowerCostText;
        [SerializeField] private int shootingTowerCost;
        [SerializeField] private int slowingTowerCost;

        private void Awake()
        {
            SetTowerCosts();
        }

        private void SetTowerCosts()
        {
            ShootingTower.cost = shootingTowerCost;
            SlowingTower.cost = slowingTowerCost;
            shootingTowerCostText.text = ShootingTower.cost.ToString() + "$";
            slowingTowerCostText.text = SlowingTower.cost.ToString() + "$";
        }

        public void placeTower(BaseTower tower, Vector3 position)
        {
            
        }

        public void sellTower(BaseTower tower)
        {
            
        }

        public bool canPlaceTower(Vector3 position)
        {
            return true;
        }
    }
}
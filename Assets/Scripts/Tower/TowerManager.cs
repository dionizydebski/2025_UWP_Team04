using System.Collections.Generic;
using Singleton;
using TMPro;
using UnityEngine;

namespace Tower
{
    public class TowerManager : Singleton<TowerManager>
    {
        private List<BaseTower> towers = new List<BaseTower>();
        
        [SerializeField] private TMP_Text shootingTowerCost;
        [SerializeField] private TMP_Text slowingTowerCost;

        private void Awake()
        {
            SetTowerCosts();
        }

        private void SetTowerCosts()
        {
            shootingTowerCost.text = ShootingTower.cost.ToString() + "$";
            slowingTowerCost.text = SlowingTower.cost.ToString() + "$";
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
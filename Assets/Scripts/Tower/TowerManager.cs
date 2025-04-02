using System.Collections.Generic;
using Singleton;
using UnityEngine;

namespace Tower
{
    public class TowerManager : Singleton<TowerManager>
    {
        private List<BaseTower> towers = new List<BaseTower>();

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
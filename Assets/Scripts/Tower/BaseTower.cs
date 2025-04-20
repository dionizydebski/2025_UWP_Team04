using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tower
{
    public abstract class BaseTower : MonoBehaviour
    {
        private List<BaseEnemy> _enemies;
        
        [Header("Statistics")] 
        [SerializeField] private TowerStats towerStats;
        
        public void Attack(BaseEnemy enemy)
        {
            
        }

        public int GetRange()
        {
            return towerStats.range;
        }
        
        public int GetCost()
        {
            return towerStats.cost;
        }

        public float GetSellModifier()
        {
            return towerStats.sellModifier;
        }

        public string GetTowerName()
        {
            return towerStats.towerName;
        }

        public virtual void UpgradeDamage()
        {
            
        }

        public virtual void UpgradeRange()
        {
            
        }

        public virtual void SetTargetStrategy(int strategyIndex)
        {
            
        }
    }
}

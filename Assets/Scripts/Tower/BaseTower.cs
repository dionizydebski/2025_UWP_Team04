using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Tower
{
    public abstract class BaseTower : MonoBehaviour
    {
        private List<BaseEnemy> _enemies;

        [Header("Statistics")] 
        [SerializeField] private string towerName;
        [SerializeField] private int range;
        [SerializeField] private float attackSpeed;
        [SerializeField] private int damage;
        [SerializeField] private float sellModifier;
        [SerializeField] private int cost;
        
        public void Attack(BaseEnemy enemy)
        {
            
        }

        public int GetRange()
        {
            return range;
        }
        
        public int GetCost()
        {
            return cost;
        }

        public float GetSellModifier()
        {
            return sellModifier;
        }

        public string GetTowerName()
        {
            return towerName;
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

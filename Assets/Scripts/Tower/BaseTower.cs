using System;
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
        
        [FormerlySerializedAs("towerStats")]
        [Header("Statistics")] 
        [SerializeField] private TowerStats baseTowerStats;
        
        private int _range;
        private float _attackSpeed;
        private int _damage;
        private float _sellModifier;
        
        private void Start()
        {
            _range = baseTowerStats.range;
            _attackSpeed = baseTowerStats.attackSpeed;
            _damage = baseTowerStats.damage;
            _sellModifier = baseTowerStats.sellModifier;
        }

        public void Attack(BaseEnemy enemy)
        {
            
        }

        public int GetBaseRange()
        {
            return baseTowerStats.range;
        }

        public int GetCurrentRange()
        {
            return _range;
        }
        
        public int GetCost()
        {
            return baseTowerStats.cost;
        }

        public float GetSellModifier()
        {
            return _sellModifier;
        }

        public string GetTowerName()
        {
            return baseTowerStats.towerName;
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

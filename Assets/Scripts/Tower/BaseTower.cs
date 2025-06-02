using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tower
{
    public abstract class BaseTower : MonoBehaviour
    {
        private const string EnemyTag = "Enemy";
        private List<BaseEnemy> _enemies;
        
        [FormerlySerializedAs("towerStats")]
        [Header("Statistics")] 
        [SerializeField] private TowerStats baseTowerStats;
        [SerializeField] private SphereCollider rangeCollider;

        protected int _range;
        protected float _attackSpeed;
        protected int _damage;
        protected float _sellModifier;
        
        [Header("Upgrades")] 
        protected int _attackLevel = 0;
        protected int _rangeLevel = 0;
        
        private int _maxAttackLevel = 2;
        private int _maxRangeLevel = 2;
        
        protected int currentUpgradeLevel = 0;
        protected int currentDamageLevel = 0;
        protected int currentRangeLevel = 0;
        
        protected float _slowModifier;
        protected float _slowDuration;

        protected List<GameObject> _enemiesInRange = new List<GameObject>();
        
        private void Start()
        {
            _range = baseTowerStats.range;
            _attackSpeed = baseTowerStats.attackSpeed;
            _damage = baseTowerStats.damage;
            _sellModifier = baseTowerStats.sellModifier;
            rangeCollider.radius = baseTowerStats.range;
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

        public int GetAttackLevel()
        {
            return _attackLevel;
        }
        
        public int GetRangeLevel()
        {
            return _rangeLevel;
        }

        public bool CanUpgradeAttack()
        {
            return _attackLevel < _maxAttackLevel;
        }

        public bool CanUpgradeRange()
        {
            return _rangeLevel < _maxRangeLevel;
        } 

        public virtual void UpgradeDamage()
        {
            
        }

        public virtual void UpgradeRange()
        {
            
        }
        
        public virtual void IncreaseAttackLevel() => _attackLevel++;
        public virtual void IncreaseRangeLevel() => _rangeLevel++;

        public virtual void SetTargetStrategy(int strategyIndex)
        {
            
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                _enemiesInRange.Add(other.gameObject);
                Debug.Log(_enemiesInRange.Count);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                Debug.Log("OnTriggerExit");
                _enemiesInRange.Remove(other.gameObject);
            }
        }
    }
}

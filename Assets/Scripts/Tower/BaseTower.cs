using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private int _range;
        private float _attackSpeed;
        private int _damage;
        private float _sellModifier;

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

        public virtual void UpgradeDamage()
        {
            
        }

        public virtual void UpgradeRange()
        {
            
        }

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

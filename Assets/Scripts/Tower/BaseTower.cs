using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Enemy;
using UnityEngine;
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
        private float _attackTimer = 0f;
        
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

        public GameObject currentModelInstance { get; set; }

        // === STRATEGIA CELU ===
        protected int currentStrategyIndex = 0;

        protected virtual void Start()
        {
            _range = baseTowerStats.range;
            _attackSpeed = baseTowerStats.attackSpeed;
            _damage = baseTowerStats.damage;
            _sellModifier = baseTowerStats.sellModifier;
            rangeCollider.radius = baseTowerStats.range;
        }
        
        public GameObject originalPrefab;

        public GameObject GetOriginalPrefab()
        {
            return originalPrefab;
        }

        protected virtual void Update()
        {
            _attackTimer += Time.deltaTime;
            
            if (_enemiesInRange.Count > 0 && _attackTimer >= _attackSpeed)
            {
                GameObject target = SelectTarget();

                if (target != null)
                {
                    Attack(target);
                    _attackTimer = 0f;
                }
                else
                {
                    _enemiesInRange.RemoveAll(e => e == null);
                }
            }
        }

        // === SELEKCJA CELU WG STRATEGII ===
        protected virtual GameObject SelectTarget()
        {
            _enemiesInRange.RemoveAll(e => e == null);

            switch (currentStrategyIndex)
            {
                case 0: // Pierwszy wróg (domyślnie)
                    return _enemiesInRange.FirstOrDefault();
                case 1: // Ostatni wróg
                    return _enemiesInRange.LastOrDefault();
                case 2: // Najbliższy
                    return _enemiesInRange.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
                case 3: // Najdalszy
                    return _enemiesInRange.OrderByDescending(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
                default:
                    return _enemiesInRange.FirstOrDefault();
            }
        }

        public void Attack(GameObject enemy)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.arrow);
            
            Projectile.Projectile projectile = TowerManager.Instance.projectilePool.Get();
            projectile.transform.position = transform.position;
            projectile.SetTarget(enemy);
            projectile.SetTower(gameObject);
            Debug.Log("Attacking");
        }

        public int GetBaseRange() => baseTowerStats.range;
        public int GetCurrentRange() => _range;
        public int GetCost() => baseTowerStats.cost;
        public int GetDamage() => _damage;
        public float GetSellModifier() => _sellModifier;
        public string GetTowerName() => baseTowerStats.towerName;

        public GameObject GetCurrentModel() => currentModelInstance;

        public int GetAttackLevel() => _attackLevel;
        public int GetRangeLevel() => _rangeLevel;

        public bool CanUpgradeAttack() => _attackLevel < _maxAttackLevel;
        public bool CanUpgradeRange() => _rangeLevel < _maxRangeLevel;

        public virtual void UpgradeDamage() { /* do nadpisania */ }
        public virtual void UpgradeRange() { /* do nadpisania */ }

        public virtual void IncreaseAttackLevel() => _attackLevel++;
        public virtual void IncreaseRangeLevel() => _rangeLevel++;

        // === STRATEGIA: set/get ===
        public virtual void SetTargetStrategy(int strategyIndex)
        {
            currentStrategyIndex = strategyIndex;
        }

        public virtual int GetCurrentStrategyIndex()
        {
            return currentStrategyIndex;
        }

        // === KOLIZJA ZASIĘGU ===
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                _enemiesInRange.Add(other.gameObject);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                _enemiesInRange.Remove(other.gameObject);
            }
        }
    }
}

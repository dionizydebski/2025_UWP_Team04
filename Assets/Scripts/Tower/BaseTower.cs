using System.Collections.Generic;
using Core;
using Tower.Strategy;
using UI.Tutorial;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tower
{
    public abstract class BaseTower : MonoBehaviour
    {
        private const string EnemyTag = "Enemy";

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
        public GameObject originalPrefab;

        protected ITargetingStrategy targetingStrategy;
        protected int currentStrategyIndex = 0;
        public static readonly string[] TargetingStrategyNames =
        {
            "Closest Target",
            "Weakest Target"
        };

        private static bool _didTutorial = false;


        protected virtual void Start()
        {
            _range = baseTowerStats.range;
            _attackSpeed = baseTowerStats.attackSpeed;
            _damage = baseTowerStats.damage;
            _sellModifier = baseTowerStats.sellModifier;
            rangeCollider.radius = baseTowerStats.range;

            SetTargetStrategy(currentStrategyIndex);
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

        protected virtual GameObject SelectTarget()
        {
            _enemiesInRange.RemoveAll(e => e == null);
            return targetingStrategy?.SelectTarget(_enemiesInRange, transform);
        }

        public void Attack(GameObject enemy)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.arrow);

            var projectile = TowerManager.Instance.projectilePool.Get();
            projectile.transform.position = transform.position;
            projectile.SetTarget(enemy);
            projectile.SetTower(gameObject);


            if (!_didTutorial)
            {
                TutorialEventsManager.Instance.TriggerNextTutorialEvent();
                TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.SetStrategyTutorialName, 0);
                _didTutorial = true;
            }
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

        public virtual void UpgradeDamage() { }
        public virtual void UpgradeRange() { }
        public virtual void UndoUpgradeDamage() { }
        public virtual void UndoUpgradeRange() { }

        public virtual void IncreaseAttackLevel() => _attackLevel++;
        public virtual void IncreaseRangeLevel() => _rangeLevel++;

        public virtual void SetTargetStrategy(int strategyIndex)
        {
            currentStrategyIndex = strategyIndex;

            switch (strategyIndex)
            {
                case 0:
                    targetingStrategy = new ClosestTargetStrategy();
                    break;
                case 1:
                    targetingStrategy = new WeakestTargetStrategy();
                    break;
                default:
                    targetingStrategy = new ClosestTargetStrategy();
                    break;
            }
        }

        public virtual int GetCurrentStrategyIndex() => currentStrategyIndex;

        public GameObject GetOriginalPrefab() => originalPrefab;

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

using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Tower
{
    //TODO:Implement Observer for AOE slow
    public class SlowingTower : BaseTower
    {
        public static int cost;
        
        [Header("Statistics")]
        [SerializeField] private float baseSlowModifier = 0.5f;
        [SerializeField] private float baseSlowDuration = 2f;
        [SerializeField] private int baseRange = 3;
        
        [SerializeField] private float slowModifierPerLevel = 0.1f;
        [SerializeField] private float slowDurationPerLevel = 0.5f;
        [SerializeField] private int rangePerLevel = 1;

        [SerializeField] private GameObject rangeVisual;

        [Header("Upgrade Settings")]
        [SerializeField] private int[] slowUpgradeCosts = { 90 };
        [SerializeField] private int[] rangeUpgradeCosts = { 75 };

        [SerializeField] private List<GameObject> upgradeModels;
        [SerializeField] private Transform modelParent;
        public static event Action<GameObject, float, float> Slowed;
        
        private void Start()
        {
            base.Start();
            UpdateStats();
            SetModelForCurrentLevel();
        }

        public void Update()
        {
            base.Update();
            foreach (var enemy in _enemiesInRange)
            {
                Slowed?.Invoke(enemy ,baseSlowModifier, baseSlowDuration);
            }
        }
        
        private void UpdateStats()
        {
            _slowModifier = baseSlowModifier + _attackLevel * slowModifierPerLevel;
            _slowDuration = baseSlowDuration + _attackLevel * slowDurationPerLevel;
            _range = baseRange + _rangeLevel * rangePerLevel;

            if (rangeVisual != null)
            {
                float scale = _range * 2f;
                rangeVisual.transform.localScale = new Vector3(scale, 1f, scale);
            }
        }
        
        public override void IncreaseAttackLevel()
        {
            base.IncreaseAttackLevel();
            UpdateStats();
        }

        public override void IncreaseRangeLevel()
        {
            base.IncreaseRangeLevel();
            UpdateStats();
        }
        
        public override void UpgradeDamage()
        {
            if (currentDamageLevel >= slowUpgradeCosts.Length) return;

            int cost = slowUpgradeCosts[currentDamageLevel];
            if (!Core.LevelManager.Instance.EnoughMoney(cost)) return;

            Core.LevelManager.Instance.SpendMoney(cost);
            currentDamageLevel++;
            currentUpgradeLevel++;
            UpdateStats();
            SetModelForCurrentLevel();
        }

        public override void UpgradeRange()
        {
            if (currentRangeLevel >= rangeUpgradeCosts.Length) return;

            int cost = rangeUpgradeCosts[currentRangeLevel];
            if (!Core.LevelManager.Instance.EnoughMoney(cost)) return;

            Core.LevelManager.Instance.SpendMoney(cost);
            currentRangeLevel++;
            currentUpgradeLevel++;
            UpdateStats();
            SetModelForCurrentLevel();
        }

        private void SetModelForCurrentLevel()
        {
            if (currentModelInstance != null)
            {
                Destroy(currentModelInstance);
            }

            if (currentUpgradeLevel < upgradeModels.Count)
            {
                currentModelInstance = Instantiate(upgradeModels[currentUpgradeLevel], modelParent);
                currentModelInstance.transform.localPosition = Vector3.zero;
                currentModelInstance.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.Log("No upgrade model available for level: " + currentUpgradeLevel);
            }
        }

        public string ProductName { get; set; }
    }
}
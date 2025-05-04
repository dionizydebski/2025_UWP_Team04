using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Tower
{
    public class RangeTower : BaseTower
    {
        public static int cost;
        private List<BaseTower> _towers = new List<BaseTower>();
        public string ProductName { get; set; }
        
        [SerializeField] private float baseDamage = 10f;
        [SerializeField] private float baseRange = 3f;

        [SerializeField] private float damagePerLevel = 5f;
        [SerializeField] private float rangePerLevel = 1f;

        [SerializeField] private GameObject rangeVisual;
        
        [SerializeField] private int[] damageUpgradeCosts = { 100, 150 };
        [SerializeField] private int[] rangeUpgradeCosts = { 80, 120 };
        
        
        private int currentDamageLevel = 0;
        private int currentRangeLevel = 0;

        private float currentDamage;
        private float currentRange;
        
        private void Start()
        {
            UpdateStats();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
        
        private void UpdateStats()
        {
            currentDamage = baseDamage + _attackLevel * damagePerLevel;
            currentRange = baseRange + _rangeLevel * rangePerLevel;

            if (rangeVisual != null)
            {
                float scale = currentRange * 2f;
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
            if (currentDamageLevel >= damageUpgradeCosts.Length) return;

            int cost = damageUpgradeCosts[currentDamageLevel];
            if (!LevelManager.Instance.EnoughMoney(cost)) return;

            LevelManager.Instance.SpendMoney(cost);
            currentDamageLevel++;
            UpdateStats();
            // upgrade modelu
        }

        public override void UpgradeRange()
        {
            if (currentRangeLevel >= rangeUpgradeCosts.Length) return;

            int cost = rangeUpgradeCosts[currentRangeLevel];
            if (!LevelManager.Instance.EnoughMoney(cost)) return;

            LevelManager.Instance.SpendMoney(cost);
            currentRangeLevel++;
            UpdateStats();
            // upgrade modelu
        }
        

    }
}
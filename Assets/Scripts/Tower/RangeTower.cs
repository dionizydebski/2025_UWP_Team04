using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Tower
{
    public class RangeTower : BaseTower
    {
        public static int cost;
        private List<BaseTower> _towers = new List<BaseTower>();
        public string ProductName { get; set; }
        
        [Header("Statistics")]
        [SerializeField] private int baseDamage = 10;
        [SerializeField] private int baseRange = 3;

        [SerializeField] private int damagePerLevel = 5;
        [SerializeField] private int rangePerLevel = 1;

        [SerializeField] private GameObject rangeVisual;
        
        [SerializeField] private int[] damageUpgradeCosts = { 100 };
        [SerializeField] private int[] rangeUpgradeCosts = { 80 };
        
        [SerializeField] private List<GameObject> upgradeModels;
        [SerializeField] private Transform modelParent;

        private void Start()
        {
            base.Start();
            UpdateStats();
            SetModelForCurrentLevel();
        }

        private void UpdateStats()
        {
            _damage = baseDamage + _attackLevel * damagePerLevel;
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
            if (currentDamageLevel >= damageUpgradeCosts.Length) return;

            int cost = damageUpgradeCosts[currentDamageLevel];
            if (!Core.LevelManager.Instance.EnoughMoney(cost)) return;

            Core.LevelManager.Instance.SpendMoney(cost);
            currentDamageLevel++;
            UpdateStats();
            currentUpgradeLevel++;
            SetModelForCurrentLevel();
            
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.UpgradeTowerTutorialName, 2);
            
        }

        public override void UpgradeRange()
        {
            if (currentRangeLevel >= rangeUpgradeCosts.Length) return;

            int cost = rangeUpgradeCosts[currentRangeLevel];
            if (!Core.LevelManager.Instance.EnoughMoney(cost)) return;

            Core.LevelManager.Instance.SpendMoney(cost);
            currentRangeLevel++;
            UpdateStats();
            currentUpgradeLevel++;
            SetModelForCurrentLevel();
            
            TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.UpgradeTowerTutorialName, 2);
            
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
                Debug.Log("No model");
            }
        }
        
        
    }
}
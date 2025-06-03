using Core;
using Core.Commands;
using Core.Commands.Core.Commands;
using TMPro; // jeśli używasz TMP_Text
using UnityEngine;
using UnityEngine.UI;
using Tower;

namespace UI
{
    public class TowerManagementPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button upgradeDamageButton;
        [SerializeField] private Button upgradeRangeButton;
        [SerializeField] private Button changeStrategyButton;
        [SerializeField] private TMP_Text strategyButtonText; // <-- jeśli używasz TMP

        private BaseTower _currentTower;

        private void Start()
        {
            panel.SetActive(false);

            upgradeDamageButton.onClick.AddListener(OnUpgradeDamage);
            upgradeRangeButton.onClick.AddListener(OnUpgradeRange);
            changeStrategyButton.onClick.AddListener(OnStrategyChanged);
        }

        public void OpenPanel(BaseTower tower)
        {
            _currentTower = tower;
            panel.SetActive(true);

            upgradeDamageButton.gameObject.SetActive(true);
            upgradeRangeButton.gameObject.SetActive(true);

            UpdateStrategyButtonText();
        }

        public void ClosePanel()
        {
            _currentTower = null;
            panel.SetActive(false);
        }

        private void OnUpgradeDamage()
        {
            if (_currentTower != null)
            {
                var command = new UpgradeDamageCommand(_currentTower);
                CommandInvoker.Instance.ExecuteCommand(command);
            }
        }

        private void OnUpgradeRange()
        {
            if (_currentTower != null)
            {
                var command = new UpgradeRangeCommand(_currentTower);
                CommandInvoker.Instance.ExecuteCommand(command);
            }
        }

        private void OnStrategyChanged()
        {
            if (_currentTower != null)
            {
                int oldIndex = _currentTower.GetCurrentStrategyIndex();
                int nextIndex = (oldIndex + 1) % BaseTower.TargetingStrategyNames.Length;

                var command = new ChangeTowerStrategyCommand(_currentTower, nextIndex);
                CommandInvoker.Instance.ExecuteCommand(command);

                UpdateStrategyButtonText();
            }
        }

        private void UpdateStrategyButtonText()
        {
            if (_currentTower != null)
            {
                int index = _currentTower.GetCurrentStrategyIndex();
                strategyButtonText.text = BaseTower.TargetingStrategyNames[index];
            }
        }
    }
}

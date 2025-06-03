using Core;
using Core.Commands;
using Core.Commands.Core.Commands;
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

        private BaseTower _currentTower;
        private CommandInvoker _commandInvoker;

        private void Awake()
        {
            _commandInvoker = FindObjectOfType<CommandInvoker>();
            if (_commandInvoker == null)
            {
                Debug.LogError("CommandInvoker not found in scene.");
            }
        }

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
                _commandInvoker.ExecuteCommand(command);
            }
        }

        private void OnUpgradeRange()
        {
            if (_currentTower != null)
            {
                var command = new UpgradeRangeCommand(_currentTower);
                _commandInvoker.ExecuteCommand(command);
            }
        }

        private void OnStrategyChanged()
        {
            if (_currentTower != null)
            {
                var command = new ChangeTowerStrategyCommand(_currentTower, 0); // <- domyślna strategia
                _commandInvoker.ExecuteCommand(command);
            }
        }
    }
}

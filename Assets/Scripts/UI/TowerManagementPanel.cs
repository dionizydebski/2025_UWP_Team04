using Core;
using Core.Commands;
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
                _currentTower.UpgradeDamage();
            }
        }

        private void OnUpgradeRange()
        {
            if (_currentTower != null)
            {
                _currentTower.UpgradeRange();
            }
        }

        private void OnStrategyChanged()
        {
            if (_currentTower != null)
            {
                ICommand command = new ChangeTowerStrategyCommand(_currentTower, 0); 
                //CommandInvoker.ExecuteCommand(command);
            }
        }
    }
}
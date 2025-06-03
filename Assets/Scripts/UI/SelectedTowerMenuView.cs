using Core;
using TMPro;
using Tower;
using UnityEngine;
using Core.Commands;

namespace UI
{
    public class SelectedTowerMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Text towerName;
        
        private CommandInvoker _commandInvoker;
        
        private void Awake()
        {
            _commandInvoker = FindObjectOfType<CommandInvoker>();
            if (_commandInvoker == null)
            {
                Debug.LogError("CommandInvoker not found in scene.");
            }
        }

        public void SetViewActive(bool show)
        {
            gameObject.SetActive(show);
        }

        public void SetTowerName(string towerName)
        {
            this.towerName.text = towerName;
        }

        public void SellTower()
        {
            BaseTower selectedTower = TowerManager.Instance.selectedTower;
            if (selectedTower == null)
            {
                Debug.LogWarning("Nie wybrano wieży do sprzedaży.");
                return;
            }

            var sellCommand = new SellTowerCommand(selectedTower);
            _commandInvoker.ExecuteCommand(sellCommand);
        }
    }
}
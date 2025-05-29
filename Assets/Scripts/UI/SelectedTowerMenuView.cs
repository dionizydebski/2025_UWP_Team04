using TMPro;
using Tower;
using UnityEngine;
using Core.Commands;

namespace UI
{
    public class SelectedTowerMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Text towerName;
        
        private CommandInvoker commandInvoker;

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
            commandInvoker.ExecuteCommand(sellCommand);
        }
    }
}
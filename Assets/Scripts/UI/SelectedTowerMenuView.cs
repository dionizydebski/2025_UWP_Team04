using TMPro;
using Tower;
using UnityEngine;

namespace UI
{
    public class SelectedTowerMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Text towerName;
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
            TowerManager.Instance.SellTower();
        }
    }
}

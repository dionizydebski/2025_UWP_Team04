using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerShopView : MonoBehaviour
    {
        [SerializeField] private TMP_Text shootingTowerCostText; 
        [SerializeField] private TMP_Text slowingTowerCostText;
        [SerializeField] private Button shootingTowerButton;
        [SerializeField] private Button slowingTowerButton;
        
        [SerializeField] private PlayerActionsController playerActionsController;
        
        public void UpdateShootingTowerCost(int cost)
        {
            shootingTowerCostText.text = cost.ToString() + "$";
        }

        public void UpdateSlowingTowerCost(int cost)
        {
            slowingTowerCostText.text = cost.ToString() + "$";
        }

        public void SelectTowerToPlace(GameObject gameObjectToPlace)
        {
            playerActionsController.SelectTowerToPlace(gameObjectToPlace);
        }

        public void ShootingTowerButtonInteractable(bool interactable)
        {
            shootingTowerButton.interactable = interactable;
        }

        public void SlowingTowerButtonInteractable(bool interactable)
        {
            slowingTowerButton.interactable = interactable;
        }
    }
}
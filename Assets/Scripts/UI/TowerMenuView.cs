using System;
using TMPro;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class TowerMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Text shootingTowerCostText; 
        [SerializeField] private TMP_Text slowingTowerCostText;
        [SerializeField] private Button shootingTowerButton;
        [SerializeField] private Button slowingTowerButton;
        
        [SerializeField] private PlayerActionsController playerActionsController;
        [SerializeField] private TowerManager towerManager;
        private GameObject _towerToPlace;
        
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
            this._towerToPlace = gameObjectToPlace;
            playerActionsController.SelectTowerToPlace(gameObjectToPlace);
        }

        public void DisableShootingTowerButton()
        {
            shootingTowerButton.interactable = false;
        }

        public void DisableSlowingTowerButton()
        {
            slowingTowerButton.interactable = false;
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TowerMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Text shootingTowerCostText; 
        [SerializeField] private TMP_Text slowingTowerCostText;

        public void UpdateShootingTowerCost(int cost)
        {
            shootingTowerCostText.text = cost.ToString() + "$";
        }

        public void UpdateSlowingTowerCost(int cost)
        {
            slowingTowerCostText.text = cost.ToString() + "$";
        }
        
    }
}
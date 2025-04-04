using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Core
{
    public class LevelStatsView : MonoBehaviour
    {
        [SerializeField] private Text healthText;
        [SerializeField] private Text moneyText;

        public void SetHealth(int health)
        {
            healthText.text = $"HP: {health}";
        }

        public void SetMoney(int money)
        {
            moneyText.text = $"$ {money}";
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelStatsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text moneyText;

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
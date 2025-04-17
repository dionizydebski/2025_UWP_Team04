using Core;
using TMPro;
using UnityEngine;

namespace UI.LevelStats
{
    public class LevelStatsView : MonoBehaviour, ILevelStatsView
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI moneyText;

        private LevelStatsPresenter _presenter;

        void Start()
        {
            _presenter = new LevelStatsPresenter(LevelManager.Instance, this);
        }

        public void UpdateHealth(int health)
        {
            healthText.text = $"HP: {health}";
        }

        public void UpdateMoney(int money)
        {
            moneyText.text = $"$ {money}";
        }
    }
}
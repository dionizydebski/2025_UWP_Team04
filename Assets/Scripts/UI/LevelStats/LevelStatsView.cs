using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI.LevelStats
{
    public class LevelStatsView : MonoBehaviour, ILevelStatsView
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI moneyText;

        private LevelStatsPresenter _presenter;

        private Coroutine healthCoroutine;
        private Coroutine moneyCoroutine;

        private void Start()
        {
            _presenter = new LevelStatsPresenter(LevelManager.Instance, this);
        }

        public void UpdateHealth(int health)
        {
            healthText.text = $"HP: {health}";

            if (healthCoroutine != null)
                StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(AnimateText(healthText, c => healthCoroutine = null));
        }

        public void UpdateMoney(int money)
        {
            moneyText.text = $"$ {money}";

            if (moneyCoroutine != null)
                StopCoroutine(moneyCoroutine);
            moneyCoroutine = StartCoroutine(AnimateText(moneyText, c => moneyCoroutine = null));
        }

        private IEnumerator AnimateText(TextMeshProUGUI textElement, System.Action<Coroutine> onFinish)
        {
            Vector3 originalScale = textElement.rectTransform.localScale;

            textElement.rectTransform.localScale = originalScale * 1.2f;

            yield return new WaitForSeconds(0.1f);

            textElement.rectTransform.localScale = originalScale;

            onFinish?.Invoke(null);
        }
    }
}
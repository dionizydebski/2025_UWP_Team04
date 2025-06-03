using Core;
using Core.Commands;
using Core.Commands.Core.Commands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tower;
using System.Collections;

namespace UI
{
    public class TowerManagementPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button upgradeDamageButton;
        [SerializeField] private Button upgradeRangeButton;
        [SerializeField] private Button changeStrategyButton;
        [SerializeField] private TMP_Text strategyButtonText;

        private BaseTower _currentTower;

        private void Start()
        {
            panel.SetActive(false);

            upgradeDamageButton.onClick.AddListener(OnUpgradeDamage);
            upgradeRangeButton.onClick.AddListener(OnUpgradeRange);
            changeStrategyButton.onClick.AddListener(OnStrategyChanged);
        }

        public void OpenPanel(BaseTower tower)
        {
            _currentTower = tower;
            panel.SetActive(true);

            upgradeDamageButton.gameObject.SetActive(true);
            upgradeRangeButton.gameObject.SetActive(true);

            UpdateStrategyButtonText();
        }

        public void ClosePanel()
        {
            _currentTower = null;
            panel.SetActive(false);
        }

        private void OnUpgradeDamage()
        {
            if (_currentTower != null)
            {
                AnimateButton(upgradeDamageButton);
                var command = new UpgradeDamageCommand(_currentTower);
                CommandInvoker.Instance.ExecuteCommand(command);
            }
        }

        private void OnUpgradeRange()
        {
            if (_currentTower != null)
            {
                AnimateButton(upgradeRangeButton);
                var command = new UpgradeRangeCommand(_currentTower);
                CommandInvoker.Instance.ExecuteCommand(command);
            }
        }

        private void OnStrategyChanged()
        {
            if (_currentTower != null)
            {
                AnimateButton(changeStrategyButton);
                int oldIndex = _currentTower.GetCurrentStrategyIndex();
                int nextIndex = (oldIndex + 1) % BaseTower.TargetingStrategyNames.Length;

                var command = new ChangeTowerStrategyCommand(_currentTower, nextIndex);
                CommandInvoker.Instance.ExecuteCommand(command);

                UpdateStrategyButtonText();
            }
        }

        private void UpdateStrategyButtonText()
        {
            if (_currentTower != null)
            {
                int index = _currentTower.GetCurrentStrategyIndex();
                strategyButtonText.text = BaseTower.TargetingStrategyNames[index];
            }
        }

        private void AnimateButton(Button button)
        {
            StartCoroutine(PunchScale(button.transform));
        }

        private IEnumerator PunchScale(Transform t)
        {
            Vector3 original = t.localScale;
            Vector3 target = original * 1.1f;
            float time = 0f;
            float duration = 0.1f;

            while (time < duration)
            {
                t.localScale = Vector3.Lerp(original, target, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            t.localScale = target;
            time = 0f;

            while (time < duration)
            {
                t.localScale = Vector3.Lerp(target, original, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            t.localScale = original;
        }
    }
}

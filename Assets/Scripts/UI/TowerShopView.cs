using System.Collections;
using TMPro;
using Tower;
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
            shootingTowerCostText.text = $"{cost}$";
        }

        public void UpdateSlowingTowerCost(int cost)
        {
            slowingTowerCostText.text = $"{cost}$";
        }

        public void SelectTowerToPlace(BaseTower towerToPlace)
        {
            // Dodaj animację kliknięcia odpowiedniego przycisku
            if (towerToPlace is RangeTower)
            {
                AnimateButton(shootingTowerButton);
            }
            else if (towerToPlace is SlowingTower)
            {
                AnimateButton(slowingTowerButton);
            }

            playerActionsController.SelectTowerToPlace(towerToPlace);
        }

        public void ShootingTowerButtonInteractable(bool interactable)
        {
            shootingTowerButton.interactable = interactable;
        }

        public void SlowingTowerButtonInteractable(bool interactable)
        {
            slowingTowerButton.interactable = interactable;
        }

        private void AnimateButton(Button button)
        {
            StartCoroutine(PunchScale(button.transform));
        }

        private IEnumerator PunchScale(Transform t)
        {
            Vector3 original = t.localScale;
            Vector3 target = original * 1.1f;
            float duration = 0.1f;
            float time = 0f;

            // Powiększenie
            while (time < duration)
            {
                t.localScale = Vector3.Lerp(original, target, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            t.localScale = target;
            time = 0f;

            // Powrót
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

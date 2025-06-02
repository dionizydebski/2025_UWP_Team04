using UnityEngine;
using Core;

namespace UI
{
    public class GameOverUIController : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;

        private void Start()
        {
            gameOverPanel.SetActive(false);
            victoryPanel.SetActive(false);

            GameManager.OnGameLost += () => gameOverPanel.SetActive(true);
            GameManager.OnGameWon += () => victoryPanel.SetActive(true);
        }
    }
}

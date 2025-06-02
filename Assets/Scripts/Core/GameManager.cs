using UnityEngine;
using Singleton;
using UI.Enemy;
using Wave;
using System;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private NextWavePreviewView previewView;
        
        public static event Action OnGameWon;
        public static event Action OnGameLost;

        private NextWavePresenter _nextWavePresenter;
        private bool _gameEnded = false;
        public bool GameEnded => _gameEnded;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _nextWavePresenter = new NextWavePresenter(WaveManager.Instance, previewView);
        }
        
        public void TriggerGameLost()
        {
            if (_gameEnded) return;
            _gameEnded = true;
            OnGameLost?.Invoke();
            Time.timeScale = 0f;
        }

        public void TriggerGameWon()
        {
            if (_gameEnded) return;
            _gameEnded = true;
            OnGameWon?.Invoke();
            Time.timeScale = 0f;
        }
    }
}
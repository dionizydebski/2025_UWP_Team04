using UnityEngine;
using Singleton;
using UI;
using UI.Enemy;
using Wave;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private NextWavePreviewView previewView;

        private NextWavePresenter _nextWavePresenter;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _nextWavePresenter = new NextWavePresenter(WaveManager.Instance, previewView);
        }
    }
}
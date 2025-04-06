using UnityEngine;
using Singleton;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private LevelStatsView view;

        private PlayerStatsPresenter presenter;

        void Start()
        {
            presenter = new PlayerStatsPresenter(LevelManager.Instance, view);
            
        }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
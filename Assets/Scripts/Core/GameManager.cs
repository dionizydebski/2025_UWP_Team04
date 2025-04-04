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
            var model = new LevelManager(100, 500);
            presenter = new PlayerStatsPresenter(model, view);
            
            presenter.DamagePlayer(10);
            presenter.GiveMoney(100);
        }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
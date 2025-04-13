using UnityEngine;
using Singleton;
using UI;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private LevelStatsView view;
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
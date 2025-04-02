using Singleton;
using Tower;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class LevelManager : Singleton<LevelManager>
    {
        public Text healthText;
        public Text coinsText;
        
        [SerializeField] private TowerManager towerManager;
    
        [Header("Stats")] 
        public static int Health;
        public static int Coins;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            healthText.text = Health.ToString();
            coinsText.text = Coins.ToString();
        }
    }
}

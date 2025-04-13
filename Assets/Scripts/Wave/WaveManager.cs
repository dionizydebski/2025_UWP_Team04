using System;
using System.Collections;
using Singleton;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Wave
{
    public class WaveManager : Singleton<WaveManager>
    {
        public static WaveManager waveManager;
        
        [Header("UI")]
        [SerializeField] private WaveUI waveUI;
        
        public Transform startPoint;
        public Transform[] path;
        
        [Header("References")] 
        [SerializeField] private GameObject[] enemyPrefabs;

        [Header("Attributes")] 
        [SerializeField] private int baseEnemies = 8;
        [SerializeField] private float enemiesPerSecond = 0.5f;
        [SerializeField] private float timeBetweenWaves = 5f;
        [SerializeField] private float difficultyScalingFactor = 0.75f;

        [Header("Events")] 
        public static UnityEvent onEnemyDestroy = new UnityEvent();

        private int _currentWave = 1;
        private float _timeSinceLastSpawn;
        private int _enemiesAlive;
        private int _enemiesLeftToSpawn;
        private bool _isSpawning = false;
        private bool _isPaused = false;
        private bool _pause = false;
        public static event Action OnWaveEnd;
        public static event Action OnWaveStart;

        private void Awake()
        {
            waveManager = this;
            onEnemyDestroy.AddListener(EnemyDestroyed);
        }

        private void Start()
        {
            if (_pause)
            {
                _isPaused = true;
                _pause = false;
                Debug.Log("Wave paused");
                return;
            }
            StartCoroutine(StartWave());
        }

        private void Update()
        {
            if(!_isSpawning || _isPaused) return;

            _timeSinceLastSpawn += Time.deltaTime;

            if (_timeSinceLastSpawn >= (1f / enemiesPerSecond) && _enemiesLeftToSpawn > 0)
            {
                SpawnEnemy();
                _enemiesLeftToSpawn--;
                _enemiesAlive++;
                _timeSinceLastSpawn = 0f;
            }

            if (_enemiesAlive == 0 && _enemiesLeftToSpawn == 0)
            {
                EndWave();
            }
        }

        private void EnemyDestroyed()
        {
            _enemiesAlive--;
        }

        private IEnumerator StartWave()
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            
            OnWaveStart?.Invoke();
            _isSpawning = true;
            _enemiesLeftToSpawn = EnemiesPerWave();
    
            if (waveUI != null)
            {
                waveUI.UpdateWaveText(_currentWave);
            }
        }

        private void EndWave()
        {
            _isSpawning = false;
            _timeSinceLastSpawn = 0f;
            _currentWave++;
            OnWaveEnd?.Invoke();
            
            if (_pause)
            {
                _isPaused = true;
                _pause = false;
                Debug.Log("Wave paused");
                return;
            }
            StartCoroutine(StartWave());
        }
        
        private void SpawnEnemy()
        {
            GameObject prefabToSpawn = enemyPrefabs[0];
            GameObject enemyInstance = Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);

            // health UI
            Canvas enemyCanvas = enemyInstance.GetComponentInChildren<Canvas>();
            if (enemyCanvas != null)
            {
                enemyCanvas.worldCamera = Camera.main;
            }
        }
        private int EnemiesPerWave()
        {
            return Mathf.RoundToInt(baseEnemies * Mathf.Pow(_currentWave, difficultyScalingFactor));
        }

        public void PauseWaveSpawning()
        {
            Debug.Log("Pause wave spawning");
            _pause = true;
        }

        public void UnpauseWaveSpawning()
        {
            if (!_isPaused) return;
            _isPaused = false;
            StartCoroutine(StartWave());
            Debug.Log("Wave unpaused");
        }
    }
}

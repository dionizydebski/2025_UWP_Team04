using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Enemy;
using Singleton;
using UI;
using UI.Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Wave
{
    public class WaveManager : Singleton<WaveManager>
    {
        public static WaveManager waveManager;

        public Transform startPoint;
        public Transform[] path;
        
        [Header("References")] 
        [SerializeField] private BaseEnemy[] enemyPrefabs;
        
        [Header("Factories")]
        [SerializeField] private NormalEnemyFactory normalEnemyFactory;
        [SerializeField] private FastEnemyFactory fastEnemyFactory;
        [SerializeField] private StrongEnemyFactory strongEnemyFactory;

        [Header("Attributes")] 
        [SerializeField] private int baseEnemies = 8;
        [SerializeField] private float enemiesPerSecond = 0.5f;
        [SerializeField] private float timeBetweenWaves = 5f;
        [SerializeField] private float difficultyScalingFactor = 0.75f;

        [Header("Events")] 
        public static UnityEvent onEnemyDestroy = new UnityEvent();
        public static event Action OnWaveEnd;
        public static event Action OnWaveStart;
        public static event Action<int> OnWaveChanged;
        public static event Action OnWavePreviewChanged;

        private int _currentWave = 1;
        private float _timeSinceLastSpawn;
        private int _enemiesAlive;
        private int _enemiesLeftToSpawn;
        private bool _isSpawning = false;
        private bool _isPaused = false;
        private bool _pause = false;
        
        public int GetCurrentWave() => _currentWave;


        protected override void Awake()
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
        }

        private void EndWave()
        {
            _isSpawning = false;
            _timeSinceLastSpawn = 0f;
            _currentWave++;
            
            OnWaveChanged?.Invoke(_currentWave);
            OnWaveEnd?.Invoke();
            OnWavePreviewChanged?.Invoke();
            TutorialEventsManager.Instance.TriggerNextTutorialEvent();
            
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
            int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            BaseEnemy prefabToSpawn = enemyPrefabs[randomIndex];
            //Debug.Log(prefabToSpawn.name);
            BaseEnemy enemyInstance;
            if (prefabToSpawn is NormalEnemy)
            {
                enemyInstance = normalEnemyFactory.CreateEnemy(startPoint);
            }
            else if (prefabToSpawn is FastEnemy)
            {
                enemyInstance = fastEnemyFactory.CreateEnemy(startPoint);
            }
            else
            {
                enemyInstance = strongEnemyFactory.CreateEnemy(startPoint);
            }

            // health UI
            Canvas enemyCanvas = enemyInstance.GameObject.GetComponentInChildren<Canvas>();
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
        
        public List<EnemyWaveInfo> GetUpcomingWavePreview()
        {
            var list = new List<EnemyWaveInfo>();

            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                int count = Mathf.RoundToInt(baseEnemies * Mathf.Pow(_currentWave, difficultyScalingFactor));
                if (count <= 0) continue;

                list.Add(new EnemyWaveInfo
                {
                    enemyName = enemyPrefabs[i].name,
                    count = count
                });
            }

            return list;
        }

        public bool isFirstEnemy()
        {
            if(_enemiesAlive == baseEnemies) return true;
            return false;
        }
    }
}

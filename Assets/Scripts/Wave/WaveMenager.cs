using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Wave
{
    public class WaveMenager : MyMonoBehaviour
    {
        public static WaveMenager waveMenager;
        
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
        private bool _isSpawing = false;

        private void Awake()
        {
            waveMenager = this;
            onEnemyDestroy.AddListener(EnemyDestroyed);
        }

        private void Start()
        {
            StartCoroutine(StartWave());
        }

        private void Update()
        {
            if(!_isSpawing) return;

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
            _isSpawing = true;
            _enemiesLeftToSpawn = EnemiesPerWave();
        }

        private void EndWave()
        {
            _isSpawing = false;
            _timeSinceLastSpawn = 0f;
            _currentWave++;
            StartCoroutine(StartWave());
        }

        private void SpawnEnemy()
        {
            GameObject prefabToSpawn = enemyPrefabs[0];
            Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);
        }
        private int EnemiesPerWave()
        {
            return Mathf.RoundToInt(baseEnemies * Mathf.Pow(_currentWave, difficultyScalingFactor));
        }
    }
}

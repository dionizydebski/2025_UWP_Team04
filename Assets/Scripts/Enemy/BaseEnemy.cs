using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Tower;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public abstract class BaseEnemy : MyMonoBehaviour
    {
        private const string EnemyTag = "Enemy";
        [Header("Stats")] 
        [SerializeField] private EnemyStats baseEnemyStats;
        [SerializeField] private ParticleSystem bloodSystem;
        private ParticleSystem _bloodParticleInstance;
        
        //Current stats
        private int _health;
        private int _damage;
        private int _moveSpeed; 
        private int _reward;
        
        private Coroutine _slowCoroutine;

        private void Start()
        {
            _health = baseEnemyStats.maxHealth;
            _damage = baseEnemyStats.damage;
            _moveSpeed = baseEnemyStats.moveSpeed;
            _reward = baseEnemyStats.reward;
            SlowingTower.Slowed += OnSlowed;
            GameObject.tag = EnemyTag;
        }
        
        private void OnSlowed(GameObject enemy, float slowModifier, float slowDuration)
        {
            if (GameObject != enemy) return;
            //Debug.Log(GameObject.name+" Slowed");
            ApplySlow(slowModifier, slowDuration);
        }
        
        private void ApplySlow(float slowAmount, float duration)
        {
            if (_slowCoroutine != null)
                StopCoroutine(_slowCoroutine);

            _slowCoroutine = StartCoroutine(SlowEffect(slowAmount, duration));
        }

        private IEnumerator SlowEffect(float slowAmount, float duration)
        {
            float originalSpeed = _moveSpeed;
            _moveSpeed = (int) (baseEnemyStats.moveSpeed * (1f - slowAmount));

            yield return new WaitForSeconds(duration);

            _moveSpeed = baseEnemyStats.moveSpeed;
            _slowCoroutine = null;
        }

        private void OnDestroy()
        {
            SlowingTower.Slowed -= OnSlowed;
        }

        public int GetHealth()
        {
            return _health;
        }

        public int GetReward()
        {
            return _reward;
        }

        public int GetSpeed()
        {
            return _moveSpeed;
        }

        public int GetDamage()
        {
            return _damage;
        }
        
        public void SpawnBlood()
        {
            Debug.Log("Spawning blood");
            _bloodParticleInstance = Instantiate(bloodSystem, GameObject.transform.position, Quaternion.identity);
        }
    }
}

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
        [FormerlySerializedAs("enemyStats")]
        [Header("Stats")] 
        [SerializeField] private EnemyStats baseEnemyStats;
        private int _health;
        private int _damage;
        private int _moveSpeed; 
        private int _reward;

        private void Awake()
        {
            _health = baseEnemyStats.maxHealth;
            _damage = baseEnemyStats.damage;
            _moveSpeed = baseEnemyStats.moveSpeed;
            _reward = baseEnemyStats.reward;
            SlowingTower.Slowed += OnSlowed;
        }

        private void OnSlowed()
        {
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {
            SlowingTower.Slowed -= OnSlowed;
        }

        public int GetReward()
        {
            return _reward;
        }

        public int GetSpeed()
        {
            return _reward;
        }

        public int GetDamage()
        {
            return _reward;
        }
    }
}

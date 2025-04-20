using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public abstract class BaseEnemy : MyMonoBehaviour
    {
        [Header("Stats")] 
        [SerializeField] private EnemyStats enemyStats;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetReward()
        {
            return enemyStats.reward;
        }

        public int GetSpeed()
        {
            return enemyStats.moveSpeed;
        }

        public int GetDamage()
        {
            return enemyStats.damage;
        }
    }
}

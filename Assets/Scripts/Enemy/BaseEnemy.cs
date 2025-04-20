using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemy : MyMonoBehaviour
    {
        [Header("Stats")]
       [SerializeField] private int damage;

        [SerializeField] private int health;

        [SerializeField] private int speed;

        [SerializeField] private int reward;

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
            return reward;
        }

        public int GetSpeed()
        {
            return speed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemy : MyMonoBehaviour
    {
        private int _damage;

        private int _health;

        private int _speed;

        private int _reward;

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
            return _reward;
        }

        public int GetSpeed()
        {
            return _speed;
        }
    }
}

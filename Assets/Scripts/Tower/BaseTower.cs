using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enemy;
using Projectile;
using UnityEngine;

namespace Tower
{
    public class BaseTower : MonoBehaviour
    {
        private List<BaseEnemy> _enemies;

        [Header("Statistics")] 
        [SerializeField] private int range;
        [SerializeField] private float attackSpeed;
        [SerializeField] private int damage;
        [SerializeField] private int cost;
        [SerializeField] private float sellModifier;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void attack(BaseEnemy enemy)
        {
            
        }
    }
}

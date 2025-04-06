using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enemy;
using Projectile;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Tower
{
    public abstract class BaseTower : MonoBehaviour
    {
        private List<BaseEnemy> _enemies;

        [Header("Statistics")] 
        [SerializeField] private int range;
        [SerializeField] private float attackSpeed;
        [SerializeField] private int damage;
        [SerializeField] private float sellModifier;
        [SerializeField] private UnityEvent select;
        
        // Start is called before the first frame update
        void Awake()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Attack(BaseEnemy enemy)
        {
            
        }

        public int GetRange()
        {
            return range;
        }

        private void OnSelect()
        {
            select?.Invoke();
        }
    }
}

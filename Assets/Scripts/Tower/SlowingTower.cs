using System;
using Core;
using UnityEngine;

namespace Tower
{
    //TODO:Implement Observer for AOE slow
    public class SlowingTower : BaseTower
    {
        public static int cost;
        [SerializeField] private float slowModifier;
        [SerializeField] private float slowDuration;
        public static event Action<GameObject, float, float> Slowed;

        public void Update()
        {
            foreach (var enemy in _enemiesInRange)
            {
                Slowed?.Invoke(enemy ,slowModifier, slowDuration);
            }
        }

        public string ProductName { get; set; }
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
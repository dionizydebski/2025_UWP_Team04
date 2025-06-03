using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tower.Strategy
{
    public class WeakestTargetStrategy : ITargetingStrategy
    {
        public GameObject SelectTarget(List<GameObject> enemiesInRange, Transform towerTransform)
        {
            return enemiesInRange
                .OrderBy(e => e.GetComponent<Enemy.BaseEnemy>()?.GetHealth() ?? float.MaxValue)
                .FirstOrDefault();
        }
    }
}
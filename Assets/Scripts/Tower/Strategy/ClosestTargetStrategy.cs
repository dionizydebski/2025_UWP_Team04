using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tower.Strategy
{
    public class ClosestTargetStrategy : ITargetingStrategy
    {
        public GameObject SelectTarget(List<GameObject> enemiesInRange, Transform towerTransform)
        {
            return enemiesInRange
                .OrderBy(e => Vector3.Distance(towerTransform.position, e.transform.position))
                .FirstOrDefault();
        }
    }
}
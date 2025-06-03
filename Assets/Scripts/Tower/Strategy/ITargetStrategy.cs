using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public interface ITargetingStrategy
    {
        GameObject SelectTarget(List<GameObject> enemiesInRange, Transform towerTransform);
    }

}
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyFactory : MonoBehaviour
    {
        public abstract BaseEnemy CreateEnemy(Transform towerTransform);
    }
}
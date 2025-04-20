using UnityEngine;

namespace Enemy
{
    public class FastEnemyFactory : BaseEnemyFactory
    {
        [SerializeField] private FastEnemy _enemyPrefab;
        
        public override BaseEnemy CreateEnemy(Transform towerTransform)
        {
            BaseEnemy enemy = Instantiate(_enemyPrefab, towerTransform);
            return enemy;
        }
    }
}
using UnityEngine;

namespace Enemy
{
    public class StrongEnemyFactory : BaseEnemyFactory
    {
        [SerializeField] private StrongEnemy _enemyPrefab;
        
        public override BaseEnemy CreateEnemy(Transform towerTransform)
        {
            BaseEnemy enemy = Instantiate(_enemyPrefab, towerTransform);
            return enemy;
        }
    }
}
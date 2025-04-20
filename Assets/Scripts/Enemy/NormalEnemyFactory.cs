using UnityEngine;

namespace Enemy
{
    public class NormalEnemyFactory : BaseEnemyFactory
    {
        [SerializeField] private NormalEnemy _enemyPrefab;
        
        public override BaseEnemy CreateEnemy(Transform towerTransform)
        {
            BaseEnemy enemy = Instantiate(_enemyPrefab, towerTransform);
            return enemy;
        }
    }
}
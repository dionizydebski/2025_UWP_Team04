using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class StrongEnemyFactory : BaseEnemyFactory
    {
        [FormerlySerializedAs("_enemyPrefab")] [SerializeField] private StrongEnemy enemyPrefab;
        
        public override BaseEnemy CreateEnemy(Transform towerTransform)
        {
            BaseEnemy enemy = Instantiate(enemyPrefab, towerTransform);
            return enemy;
        }
    }
}
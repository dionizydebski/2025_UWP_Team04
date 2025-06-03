using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class FastEnemyFactory : BaseEnemyFactory
    {
        [FormerlySerializedAs("_enemyPrefab")] [SerializeField] private FastEnemy enemyPrefab;
        
        public override BaseEnemy CreateEnemy(Transform towerTransform)
        {
            BaseEnemy enemy = Instantiate(enemyPrefab, towerTransform);
            return enemy;
        }
    }
}
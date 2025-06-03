using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class NormalEnemyFactory : BaseEnemyFactory
    {
        [FormerlySerializedAs("_enemyPrefab")] [SerializeField] private NormalEnemy enemyPrefab;
        
        public override BaseEnemy CreateEnemy(Transform towerTransform)
        {
            BaseEnemy enemy = Instantiate(enemyPrefab, towerTransform);
            return enemy;
        }
    }
}
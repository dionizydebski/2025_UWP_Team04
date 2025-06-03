using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
    public class EnemyStats : ScriptableObject
    {
        [Header("Stats")]
        public int maxHealth;
        public int damage;
        public int moveSpeed; 
        public int reward;

        [Header("References")] 
        public GameObject enemyPrefab;
    }
}
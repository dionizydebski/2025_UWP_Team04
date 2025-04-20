using UnityEngine;

namespace Tower
{
    [CreateAssetMenu(fileName = "TowerStatsScriptableObject", menuName = "ScriptableObjects/TowerStats", order = 2)]
    public class TowerStats : ScriptableObject
    {
        [Header("Statistics")] 
        public string towerName;
        public int range;
        public float attackSpeed;
        public int damage;
        public float sellModifier;
        public int cost;
        
        [Header("References")]
        public GameObject towerPrefab;
    }
}
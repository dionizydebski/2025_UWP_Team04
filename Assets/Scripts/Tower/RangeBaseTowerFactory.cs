using UnityEngine;

namespace Tower
{
    public class RangeBaseTowerFactory : BaseTowerFactory
    {
        [SerializeField] private RangeTower _towerPrefab;
        public override BaseTower CreateTower(Transform towerTransform)
        {
            RangeTower tower = Instantiate(_towerPrefab, towerTransform);
            return tower;
        }
    }
}
using Core;
using UnityEngine;

namespace Tower
{
    public class SlowingBaseTowerFactory : BaseTowerFactory
    {
        [SerializeField] SlowingTower _towerPrefab;

        public override BaseTower CreateTower(Transform towerTransform)
        {
            SlowingTower tower = Instantiate(_towerPrefab, towerTransform);
            return tower;
        }
    }
}
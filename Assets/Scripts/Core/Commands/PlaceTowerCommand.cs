using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class PlaceTowerCommand : ICommand
    {
        private BaseTower _towerPrefab;
        private Vector3 _position;
        private BaseTower _placedTower;

        public PlaceTowerCommand(BaseTower prefab, Vector3 pos)
        {
            _towerPrefab = prefab;
            _position = pos;
        }

        public void Execute()
        {
            _placedTower = TowerManager.Instance.PlaceTowerAndReturn(_towerPrefab, _position);
        }

        public void Undo()
        {
            if (_placedTower != null)
            {
                TowerManager.Instance.RefundTower(_placedTower);
                TowerManager.Instance.RemoveTower(_placedTower);
            }
        }
    }

}
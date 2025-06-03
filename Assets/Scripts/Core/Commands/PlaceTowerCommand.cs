using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class PlaceTowerCommand : ICommand
    {
        private BaseTower towerPrefab;
        private Vector3 position;
        private BaseTower placedTower;

        public PlaceTowerCommand(BaseTower prefab, Vector3 pos)
        {
            towerPrefab = prefab;
            position = pos;
        }

        public void Execute()
        {
            placedTower = TowerManager.Instance.PlaceTowerAndReturn(towerPrefab, position);
        }

        public void Undo()
        {
            if (placedTower != null)
            {
                TowerManager.Instance.RefundTower(placedTower);
                TowerManager.Instance.RemoveTower(placedTower);
            }
        }
    }

}
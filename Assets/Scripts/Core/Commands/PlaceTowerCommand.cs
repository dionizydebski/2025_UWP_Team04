using Tower;
using UnityEngine;

namespace Core
{
    public class PlaceTowerCommand : ICommand
    {
        private BaseTower towerPrefab;
        private Vector3 position;
        private BaseTower placedTower;

        public PlaceTowerCommand(BaseTower towerPrefab, Vector3 position)
        {
            this.towerPrefab = towerPrefab;
            this.position = position;
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
                GameObject.Destroy(placedTower.gameObject);
            }
        }
    }
}
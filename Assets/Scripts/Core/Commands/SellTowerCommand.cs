using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class SellTowerCommand : ICommand
    {
        private BaseTower towerToSell;
        private BaseTower towerPrefab;
        private Vector3 position;

        public SellTowerCommand(BaseTower tower)
        {
            towerToSell = tower;
            towerPrefab = TowerManager.Instance.GetPrefabForTower(tower);
            position = tower.transform.position;
        }

        public void Execute()
        {
            TowerManager.Instance.RefundTower(towerToSell);
            TowerManager.Instance.RemoveTower(towerToSell);
        }

        public void Undo()
        {
            TowerManager.Instance.PlaceTowerAndReturn(towerPrefab, position);
        }
    }

}
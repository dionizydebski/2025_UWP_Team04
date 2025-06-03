using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class SellTowerCommand : ICommand
    {
        private BaseTower _towerToSell;
        private BaseTower _towerPrefab;
        private Vector3 _position;

        public SellTowerCommand(BaseTower tower)
        {
            _towerToSell = tower;
            _towerPrefab = TowerManager.Instance.GetPrefabForTower(tower);
            _position = tower.transform.position;
        }

        public void Execute()
        {
            TowerManager.Instance.SellTower(_towerToSell);
            TowerManager.Instance.RemoveTower(_towerToSell);
        }

        public void Undo()
        {
            TowerManager.Instance.PlaceTowerAndReturnWithRefundCost(_towerPrefab, _position);
        }
    }

}
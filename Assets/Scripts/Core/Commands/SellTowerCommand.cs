using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class SellTowerCommand : ICommand
    {
        private readonly BaseTower towerInstance;
        private readonly Vector3 position;
        private readonly BaseTower towerPrefab;
        private int refundAmount;

        private BaseTower restoredTower;

        public SellTowerCommand(BaseTower tower)
        {
            towerInstance = tower;
            position = tower.transform.position;
            towerPrefab = TowerManager.Instance.GetPrefabForTower(tower);
        }

        public void Execute()
        {
            if (towerInstance != null)
            {
                refundAmount = Mathf.RoundToInt(towerInstance.GetCost() * towerInstance.GetSellModifier());
                TowerManager.Instance.RefundTower(towerInstance);
                Object.Destroy(towerInstance.gameObject);
            }
        }

        public void Undo()
        {
            if (towerPrefab != null)
            {
                restoredTower = TowerManager.Instance.PlaceTowerAndReturn(towerPrefab, position);
                // Można tu dodać przywracanie stanu, jeśli to potrzebne
            }
        }
    }
}
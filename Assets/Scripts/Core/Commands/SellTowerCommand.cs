using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class SellTowerCommand : ICommand
    {
        private readonly BaseTower tower;
        private readonly Vector3 position;
        private int refundAmount;

        public SellTowerCommand(BaseTower tower)
        {
            this.tower = tower;
            this.position = tower.transform.position;
        }

        public void Execute()
        {
            refundAmount = (int)(tower.GetCost() * tower.GetSellModifier());
            TowerManager.Instance.RefundTower(tower);
            Object.Destroy(tower.gameObject);
        }

        public void Undo()
        {
            // Można zaimplementować ponowne postawienie wieży jeśli chcesz pełne Undo
            Debug.LogWarning("Undo for SellTowerCommand not fully implemented (no restore logic).");
        }
    }
}
using Core;
using Tower;
using UnityEngine;

namespace Core.Commands
{
    public class SellTowerCommand : ICommand
    {
        private GameObject towerPrefab;
        private Vector3 position;
        private int refundAmount;
        private BaseTower towerInstance;

        public SellTowerCommand(BaseTower tower)
        {
            towerPrefab = tower.GetOriginalPrefab(); // ważne!
            position = tower.transform.position;
            refundAmount = tower.GetCost();
            towerInstance = tower;
        }

        public void Execute()
        {
            LevelManager.Instance.AddMoney(refundAmount);
            TowerManager.Instance.SellTower();
        }

        public void Undo()
        {
            if (towerPrefab != null)
            {
                GameObject newObj = GameObject.Instantiate(towerPrefab, position, Quaternion.identity);
                BaseTower newTower = newObj.GetComponent<BaseTower>();
                LevelManager.Instance.SpendMoney(refundAmount);
                TowerManager.Instance.RegisterTower(newTower, towerPrefab.GetComponent<BaseTower>());
            }
            else
            {
                Debug.LogError("SellTowerCommand: towerPrefab is null, cannot undo.");
            }
        }

    }
}
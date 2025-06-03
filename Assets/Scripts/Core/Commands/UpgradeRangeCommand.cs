using Tower;

namespace Core.Commands
{
    public class UpgradeRangeCommand : ICommand
    {
        private readonly BaseTower tower;

        public UpgradeRangeCommand(BaseTower tower)
        {
            this.tower = tower;
        }

        public void Execute()
        {
            tower.UpgradeRange();
        }

        public void Undo()
        {
            tower.UndoUpgradeRange();
        }
    }
}
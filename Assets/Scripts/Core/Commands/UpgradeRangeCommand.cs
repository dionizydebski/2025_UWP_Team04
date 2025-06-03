using Tower;

namespace Core.Commands
{
    public class UpgradeRangeCommand : ICommand
    {
        private readonly BaseTower _tower;

        public UpgradeRangeCommand(BaseTower tower)
        {
            _tower = tower;
        }

        public void Execute()
        {
            _tower.UpgradeRange();
        }

        public void Undo()
        {
            _tower.UndoUpgradeRange();
        }
    }
}
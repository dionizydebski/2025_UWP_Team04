using Tower;

namespace Core.Commands
{
    namespace Core.Commands
    {
        public class UpgradeDamageCommand : ICommand
        {
            private readonly BaseTower _tower;

            public UpgradeDamageCommand(BaseTower tower)
            {
                _tower = tower;
            }

            public void Execute()
            {
                _tower.UpgradeDamage();
            }

            public void Undo()
            {
                _tower.UndoUpgradeDamage();
            }
        }
    }
}
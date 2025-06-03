using Tower;

namespace Core.Commands
{
    namespace Core.Commands
    {
        public class UpgradeDamageCommand : ICommand
        {
            private readonly BaseTower tower;

            public UpgradeDamageCommand(BaseTower tower)
            {
                this.tower = tower;
            }

            public void Execute()
            {
                tower.UpgradeDamage();
            }

            public void Undo()
            {
                tower.UndoUpgradeDamage();
            }
        }
    }
}
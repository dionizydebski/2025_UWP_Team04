using Tower;

namespace Core.Commands
{
    public class ChangeTowerStrategyCommand : ICommand
    {
        private readonly BaseTower _tower;
        private readonly int _newStrategyIndex;
        private int _oldStrategyIndex;

        public ChangeTowerStrategyCommand(BaseTower tower, int newStrategyIndex)
        {
            _tower = tower;
            _newStrategyIndex = newStrategyIndex;
        }

        public void Execute()
        {
            if (_tower != null)
            {
                _oldStrategyIndex = _tower.GetCurrentStrategyIndex();
                _tower.SetTargetStrategy(_newStrategyIndex);
            }
        }

        public void Undo()
        {
            if (_tower != null)
            {
                _tower.SetTargetStrategy(_oldStrategyIndex);
            }
        }
    }
}
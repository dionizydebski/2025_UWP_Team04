using Tower;

namespace Core.Commands
{
    public class ChangeTowerStrategyCommand : ICommand
    {
        private readonly BaseTower _tower;
        private readonly int _newStrategyIndex;
        private int _oldStrategyIndex;
        private bool _hasExecuted = false;

        public ChangeTowerStrategyCommand(BaseTower tower, int newStrategyIndex)
        {
            _tower = tower;
            _newStrategyIndex = newStrategyIndex;
        }

        public void Execute()
        {
            if (_tower != null && !_hasExecuted)
            {
                _oldStrategyIndex = _tower.GetCurrentStrategyIndex();
                _hasExecuted = true;
            }

            _tower?.SetTargetStrategy(_newStrategyIndex);
        }

        public void Undo()
        {
            if (_tower != null && _hasExecuted)
            {
                _tower.SetTargetStrategy(_oldStrategyIndex);
            }
        }
    }
}

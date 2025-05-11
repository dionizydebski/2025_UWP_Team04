using Tower;

namespace Core.Commands
{
    public class ChangeTowerStrategyCommand : ICommand
    {
        private BaseTower _tower;
        private int _strategyIndex;

        public ChangeTowerStrategyCommand(BaseTower tower, int strategyIndex)
        {
            _tower = tower;
            _strategyIndex = strategyIndex;
        }

        public void Execute()
        {
            if (_tower != null)
            {
                _tower.SetTargetStrategy(_strategyIndex);
            }
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }
    }
}
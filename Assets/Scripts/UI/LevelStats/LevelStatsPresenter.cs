namespace Core
{
    public class LevelStatsPresenter
    {
        private readonly LevelManager _model;
        private readonly ILevelStatsView _view;

        public LevelStatsPresenter(LevelManager model, ILevelStatsView view)
        {
            _model = model;
            _view = view;
            
            _model.OnHealthChanged += _view.UpdateHealth;
            _model.OnMoneyChanged += _view.UpdateMoney;
            
            _view.UpdateHealth(_model.GetHealth());
            _view.UpdateMoney(_model.GetMoney());
        }
    }
}
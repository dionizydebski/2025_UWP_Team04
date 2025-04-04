namespace Core
{

    public class PlayerStatsPresenter
    {
        private LevelManager model;
        private LevelStatsView view;

        public PlayerStatsPresenter(LevelManager model, LevelStatsView view)
        {
            this.model = model;
            this.view = view;

            model.OnHealthChanged += view.SetHealth;
            model.OnMoneyChanged += view.SetMoney;
            
            view.SetHealth(model.Health);
            view.SetMoney(model.Money);
        }

        public void DamagePlayer(int amount)
        {
            model.TakeDamage(amount);
        }

        public void GiveMoney(int amount)
        {
            model.AddMoney(amount);
        }
    }
}
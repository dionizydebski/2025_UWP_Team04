namespace Core
{
    public interface ILevelStatsView
    {
        void UpdateHealth(int health);
        void UpdateMoney(int money);
    }
}
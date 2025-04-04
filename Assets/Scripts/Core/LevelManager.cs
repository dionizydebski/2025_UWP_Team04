using System;

namespace Core
{
    public class LevelManager : Singleton<LevelManager>
    {
        public int Health { get; private set; }
        public int Money { get; private set; }

        public event Action<int> OnHealthChanged;
        public event Action<int> OnMoneyChanged;

        public LevelManager(int initialHealth, int initialMoney)
        {
            Health = initialHealth;
            Money = initialMoney;
        }
        
        public void TakeDamage(int amount)
        {
            Health -= amount;
            OnHealthChanged?.Invoke(Health);
        }

        public void AddMoney(int amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(Money);
        }

    }
}

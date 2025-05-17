using System;
using Singleton;
using UnityEngine;

namespace Core
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private int initialHealth = 100;
        [SerializeField] private int initialMoney = 500;
        
        public int GetHealth() => Health;
        public int GetMoney() => Money;
        
        public int Health { get; private set; }
        public int Money { get; private set; }

        public event Action<int> OnHealthChanged;
        public event Action<int> OnMoneyChanged;

        protected override void Awake()
        {
            base.Awake();
            Health = initialHealth;
            Money = initialMoney;
        }
        
        public void TakeDamage(int amount)
        {
            Health -= amount;
            OnHealthChanged?.Invoke(Health);
            
            AudioManager.Instance.PlaySFX(AudioManager.Instance.damageToBase);
            
            AudioManager.Instance.musicSource.pitch += amount/100f;
        }

        public void AddMoney(int amount)
        {
            Money += amount;
            OnMoneyChanged?.Invoke(Money);
        }

        public bool EnoughMoney(int amount)
        {
            if (Money >= amount)
            {
                return true;
            }
            return false;
        }

        public void SpendMoney(int amount)
        {
            Money -= amount;
            OnMoneyChanged?.Invoke(Money);
        }

    }
}

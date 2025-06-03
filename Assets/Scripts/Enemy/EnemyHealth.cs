using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        private int _currentHealth;

        [SerializeField] private Slider healthSlider;

        private void Start()
        {
            _currentHealth = maxHealth;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            //Debug.Log("Current health " + _currentHealth);
            GetComponent<BaseEnemy>().SpawnBlood();
            _currentHealth -= damage;
            healthSlider.value = _currentHealth;
            
            AudioManager.Instance.PlaySFX(AudioManager.Instance.arrowHit);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Core.LevelManager.Instance.AddMoney(GetComponent<BaseEnemy>().GetReward());
            Wave.WaveManager.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
        
        
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        private int _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

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
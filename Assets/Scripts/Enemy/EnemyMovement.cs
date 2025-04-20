using Core;
using UnityEngine;
using Wave;

namespace Enemy
{
    public class EnemyMovement : MyMonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Rigidbody rb;

        private Transform _target;
        private int _pathIndex = 0;
        private BaseEnemy _baseTowerComponent;

        void Start()
        {
            _target = WaveManager.waveManager.path[_pathIndex];
            _baseTowerComponent = GetComponent<BaseEnemy>();
        }
    
        private void Update()
        {
            if (Vector3.Distance(_target.position, Transform.position) <= 0.1f)
            {
                _pathIndex++;
                if (_pathIndex == WaveManager.waveManager.path.Length)
                {
                    WaveManager.onEnemyDestroy.Invoke();
                    LevelManager.Instance.TakeDamage(_baseTowerComponent.GetDamage());
                    Destroy(GameObject);
                }
                else
                {
                    _target = WaveManager.waveManager.path[_pathIndex];
                }
            }
        }

        private void FixedUpdate()
        {
            Vector3 direction = (_target.position - Transform.position).normalized;
            rb.velocity = direction * _baseTowerComponent.GetSpeed();
        }
    }
}

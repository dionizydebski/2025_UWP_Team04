using Core;
using UnityEngine;
using Wave;

namespace Enemy
{
    public class EnemyMovement : MyMonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Rigidbody rb;
    
        [Header("Attributes")] 
        [SerializeField] private float moveSpeed = 2f;

        private Transform _target;
        private int _pathIndex = 0;
    
        void Start()
        {
            _target = WaveManager.waveManager.path[_pathIndex];
            moveSpeed = GetComponent<BaseEnemy>().GetSpeed();
        }
    
        private void Update()
        {
            if (Vector3.Distance(_target.position, Transform.position) <= 0.1f)
            {
                _pathIndex++;
                if (_pathIndex == WaveManager.waveManager.path.Length)
                {
                    WaveManager.onEnemyDestroy.Invoke();
                    LevelManager.Instance.TakeDamage(1);
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
            rb.velocity = direction * moveSpeed;
        }
    }
}

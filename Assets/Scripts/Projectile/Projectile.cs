using Enemy;
using Tower;
using UnityEngine;
using UnityEngine.Pool;

namespace Projectile
{
    public class Projectile : MonoBehaviour
    {
        private const string EnemyTag = "Enemy";
        private const float MaxLifeTime = 3f;
        
        public IObjectPool<Projectile> Pool { get; set; }
        
        private Rigidbody _rb;
        private float _lifeTimer = MaxLifeTime;
        
        public GameObject tower;
        public GameObject target;
        public float speed = 10f;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
        }
        public void ResetState() {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            _lifeTimer = MaxLifeTime;
            target = null;
            tower = null;
        }

        private void Update() {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0f || target == null) {
                ReturnToPool();
            }
            
            if (target != null) {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.position += direction * (speed * Time.deltaTime);
                transform.forward = direction;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(EnemyTag)) return;
            target.GetComponent<EnemyHealth>().TakeDamage(tower.GetComponent<BaseTower>().GetDamage());
            ReturnToPool();
        }
        private void ReturnToPool() {
            if (Pool!= null) {
                Pool.Release(this);
            } else {
                Destroy(gameObject); }
        }
        
        public void SetTarget(GameObject enemy)
        {
            target = enemy;
        }
        public void SetTower(GameObject tower)
        {
            this.tower = tower;
        }
    }
}
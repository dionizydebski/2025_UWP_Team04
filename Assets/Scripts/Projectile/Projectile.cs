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
        private Rigidbody rb;
        private float lifeTimer = MaxLifeTime;
        public GameObject tower;
        public GameObject target;
        public float speed = 10f;
        void Awake() {
            rb = GetComponent<Rigidbody>();
        }
        public void ResetState() {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            lifeTimer = MaxLifeTime;
            target = null;
            tower = null;
        }

        void Update() {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f || target == null) {
                ReturnToPool();
            }
            
            if (target != null) {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                transform.forward = direction;
            }
        }
        void OnTriggerEnter(Collider other) {
            // Here add collision logic
            if (other.CompareTag(EnemyTag))
            {
                //Debug.Log("Zadaje obrazenia");
                target.GetComponent<EnemyHealth>().TakeDamage(tower.GetComponent<BaseTower>().GetDamage());
                ReturnToPool();
            }
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
using Core;
using UnityEngine;
using Wave;

namespace Enemy
{
    public class EnemyMovement : MyMonoBehaviour
    {
        [Header("References")] 
        [SerializeField] public Rigidbody rb;

        public Transform _target;
        private int _pathIndex = 0;
        public BaseEnemy _baseTowerComponent;
        
        private StateMachine _enemyStateMachine;
        public StateMachine EnemyStateMachine => _enemyStateMachine;
        
        private void Awake() {
            // ...
            _enemyStateMachine= new StateMachine(this);
        }

        void Start()
        {
            _target = WaveManager.Instance.path[_pathIndex];
            _baseTowerComponent = GetComponent<BaseEnemy>();
            
            _enemyStateMachine.Initialize(_enemyStateMachine.walkState);
        }
    
        private void Update()
        {
            _enemyStateMachine.Update();
            if (Vector3.Distance(_target.position, Transform.position) <= 0.1f)
            {
                _pathIndex++;
                if (_pathIndex == WaveManager.Instance.path.Length)
                {
                    WaveManager.onEnemyDestroy.Invoke();
                    
                    if(LevelManager.Instance.GetHealth() == 100)
                    {
                        TutorialEventsManager.Instance.TriggerNextTutorialEvent();
                        TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.EnemyAttackTutorialName, 0);
                    }
                    _enemyStateMachine.TransitionTo(_enemyStateMachine.damageState);
                    Destroy(GameObject);
                }
                else
                {
                    _target = WaveManager.Instance.path[_pathIndex];
                }
            }
        }
    }
}

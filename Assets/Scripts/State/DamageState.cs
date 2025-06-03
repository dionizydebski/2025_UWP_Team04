using Core;
using Enemy;

namespace State
{
    public class DamageState : IState
    {
        private EnemyMovement enemy;
        public DamageState(EnemyMovement enemy) {
            this.enemy = enemy;
        }

        public void Enter()
        {
            LevelManager.Instance.TakeDamage(enemy._baseTowerComponent.GetDamage());
        }
        public void Update() { }
        public void Exit() { }
    }
}

using System;
using Enemy;

namespace State
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        public WalkState walkState;
        public DamageState damageState;
        public event Action<IState> StateChanged;
        public StateMachine(EnemyMovement enemy) {
            this.walkState = new WalkState(enemy);
            this.damageState = new DamageState(enemy);
        }
        public void Initialize(IState state) {
            CurrentState = state;
            state.Enter();
            StateChanged?.Invoke(state);
        }

        public void TransitionTo(IState nextState) {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
            StateChanged?.Invoke(nextState);
        }
        public void Update() {
            if (CurrentState != null) {
                CurrentState.Update();
            }
        }
    }
}

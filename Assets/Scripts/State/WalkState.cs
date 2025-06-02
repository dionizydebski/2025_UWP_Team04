using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class WalkState : IState
{
    private EnemyMovement enemy;
    public WalkState(EnemyMovement enemy) {
        this.enemy = enemy;
    }
    public void Enter() { }

    public void Update()
    {
        Vector3 direction = (enemy._target.position - enemy.Transform.position).normalized;
        enemy.rb.velocity = direction * enemy._baseTowerComponent.GetSpeed();
    }
    public void Exit() { }
}

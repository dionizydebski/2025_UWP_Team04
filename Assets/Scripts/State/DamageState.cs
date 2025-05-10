using System.Collections;
using System.Collections.Generic;
using Core;
using Enemy;
using UnityEngine;

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

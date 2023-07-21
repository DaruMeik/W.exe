using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillState : EnemyBaseState
{
    private float nextAimTime = 0;
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.aimPoint = enemy.target.position;
        nextAimTime = Time.time + enemy.enemyStat.enemyAimTime[0];
        enemy.animator.SetTrigger("Shoot");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        if(Time.time > nextAimTime)
        {
            enemy.aimPoint = enemy.target.position;
            nextAimTime = Time.time + enemy.enemyStat.enemyAimTime[0];
        }
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.nextTimeToUseSkill = Time.time + enemy.enemyStat.enemyCD[0];
    }
}

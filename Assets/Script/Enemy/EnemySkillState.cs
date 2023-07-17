using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.animator.SetTrigger("Shoot");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.nextTimeToUseSkill = Time.time + enemy.enemyStat.enemyCD[0];
    }
}

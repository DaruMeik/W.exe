using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.path != null)
        {
            if (enemy.currentWayPoint >= enemy.path.vectorPath.Count)
            {
                enemy.isEndOfPath = true;
                enemy.rb.velocity = Vector2.zero;
                return;
            }
            else
            {
                enemy.isEndOfPath = false;
            }

            Vector2 moveDir = ((Vector2)enemy.path.vectorPath[enemy.currentWayPoint] - enemy.rb.position).normalized;
            enemy.rb.velocity = moveDir * enemy.enemyStat.enemyMovementSpeed;
            if (moveDir.x > 0.05f)
            {
                enemy.enemySprite.flipX = false;
            }
            else if (moveDir.x < -0.05f)
            {
                enemy.enemySprite.flipX = true;
            }

            float distance = Vector2.Distance(enemy.scanPoint.position, enemy.path.vectorPath[enemy.currentWayPoint]);

            if (distance < enemy.enemyStat.enemyAtkRange[0] && Time.time > enemy.nextTimeToUseSkill)
            {
                enemy.SwitchState(enemy.skillState);
            }
            if (distance < enemy.nextWayPointDistance)
            {
                enemy.currentWayPoint++;
            }
        }
    }
    public override void ExitState(EnemyStateManager enemy)
    {
    }
}

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

            float distanceToNextWayPoint = Vector2.Distance(enemy.scanPoint.position, enemy.path.vectorPath[enemy.currentWayPoint]);
            float distanceToTarget = enemy.path.GetTotalLength();

            if (distanceToTarget < enemy.enemyStat.enemyAtkRange[0] && Time.time > enemy.nextTimeToUseSkill)
            {
                if (!enemy.enemyStat.requireLOS[0] || (enemy.enemyStat.requireLOS[0] && !Physics2D.Linecast(enemy.scanPoint.position, enemy.target.position, LayerMask.GetMask("Wall"))))
                    enemy.SwitchState(enemy.skillState);
            }
            if (distanceToNextWayPoint < enemy.nextWayPointDistance)
            {
                enemy.currentWayPoint++;
            }
        }
    }
    public override void ExitState(EnemyStateManager enemy)
    {
    }
}

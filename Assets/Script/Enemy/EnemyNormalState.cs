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
                enemy.path = null;
                enemy.isEndOfPath = true;
                enemy.rb.velocity = Vector2.zero;
                return;
            }
            else
            {
                enemy.isEndOfPath = false;
            }

            Vector2 moveDir = ((Vector2)enemy.path.vectorPath[enemy.currentWayPoint] - enemy.rb.position).normalized;
            if(enemy.enemyStat.enemyBehavior == "Patrol" && enemy.provoked)
            {
                enemy.rb.velocity = moveDir * enemy.enemyStat.enemyMovementSpeed * 2f;
            }
            else
            {
                enemy.rb.velocity = moveDir * enemy.enemyStat.enemyMovementSpeed;
            }
            if (moveDir.x > 0.1f)
            {
                enemy.enemySprite.transform.localScale =  new Vector3(Mathf.Abs(enemy.enemySprite.transform.localScale.x), enemy.enemySprite.transform.localScale.y, enemy.enemySprite.transform.localScale.z);
            }
            else if (moveDir.x < -0.1f)
            {
                enemy.enemySprite.transform.localScale = new Vector3(-Mathf.Abs(enemy.enemySprite.transform.localScale.x), enemy.enemySprite.transform.localScale.y, enemy.enemySprite.transform.localScale.z);
            }

            float distanceToNextWayPoint = Vector2.Distance(enemy.scanPoint.position, enemy.path.vectorPath[enemy.currentWayPoint]);
            float distanceToTarget = Vector2.Distance(enemy.scanPoint.position, enemy.target.position);

            if (distanceToTarget < enemy.enemyStat.enemyAtkRange[0] && Time.time > enemy.nextTimeToUseSkill && enemy.provoked)
            {
                if (!enemy.enemyStat.requireLOS[0] || (enemy.enemyStat.requireLOS[0] && !Physics2D.Linecast(enemy.scanPoint.position, enemy.target.position, LayerMask.GetMask("Wall"))))
                {
                    enemy.skillState.fixedTarget = null;
                    enemy.SwitchState(enemy.skillState);
                }
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

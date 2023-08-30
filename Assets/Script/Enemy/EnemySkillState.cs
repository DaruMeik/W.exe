using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillState : EnemyBaseState
{
    private float nextAimTime = 0;
    public Transform fixedTarget = null;
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.rb.velocity = Vector2.zero;
        if(fixedTarget == null)
            enemy.aimPoint = enemy.target.position + new Vector3(0f, 0.5f, 0f);
        else
            enemy.aimPoint = fixedTarget.position;
        nextAimTime = Time.time + enemy.enemyStat.enemyAimTime[0];
        enemy.animator.SetTrigger("Shoot");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        if(Time.time > nextAimTime && fixedTarget == null)
        {
            enemy.aimPoint = enemy.target.position + new Vector3(0f,0.5f,0f);
            if (enemy.aimPoint.x - enemy.transform.position.x > 0.1f)
            {
                enemy.enemySprite.transform.localScale = new Vector3(Mathf.Abs(enemy.enemySprite.transform.localScale.x), enemy.enemySprite.transform.localScale.y, enemy.enemySprite.transform.localScale.z);
            }
            else if (enemy.aimPoint.x - enemy.transform.position.x < -0.1f)
            {
                enemy.enemySprite.transform.localScale = new Vector3(-Mathf.Abs(enemy.enemySprite.transform.localScale.x), enemy.enemySprite.transform.localScale.y, enemy.enemySprite.transform.localScale.z);
            }
            nextAimTime = Time.time + enemy.enemyStat.enemyAimTime[0];
        }
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.StopSkill();
        enemy.nextTimeToUseSkill = Time.time + enemy.enemyStat.enemyCD[0];
    }
}

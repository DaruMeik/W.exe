using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    public float stunDuration = 0f;
    private float outOfStunTime = 0f;
    private float flashTime;
    private bool show = true;
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.StopSkill();
        enemy.animator.Play("Walk");
        enemy.enemySprite.color = new Color32(255, 255, 220, 255);
        flashTime = Time.time;
        show = true;
        outOfStunTime = Time.time + stunDuration;
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        if (Time.time - flashTime >= 0.1f)
        {
            if (show)
            {
                enemy.enemySprite.material = enemy.defaultMat;
            }
            else
            {
                enemy.enemySprite.material = enemy.whiteFlashMat;
            }
            show = !show;
            flashTime = Time.time;
        }
        if (Time.time > outOfStunTime)
        {
            enemy.SwitchState(enemy.normalState);
        }
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.nextTimeToUseSkill = Time.time + enemy.enemyStat.enemyCD[0];
        enemy.enemySprite.color = new Color32(255, 255, 255, 255);
    }
}

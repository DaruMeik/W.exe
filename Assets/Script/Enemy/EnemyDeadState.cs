using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private float countTime;
    private float flashTime;
    private bool show = true;
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.StopSkill();
        enemy.enemyCollider.enabled = false;
        enemy.rb.velocity = Vector2.zero;
        enemy.enemySprite.color = new Color32(255, 200, 200, 255);
        flashTime = Time.time;
        countTime = Time.time;
        if(enemy.marked)
            enemy.possessionCollider.SetActive(true);
        enemy.animator.SetTrigger("Dead");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        if (Time.time - flashTime >= 0.15f)
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
        if ((Time.time - countTime >= 2f && !enemy.isPossessed) || !enemy.marked)
        {
            enemy.isExploding = true;
            enemy.animator.SetTrigger("Explode");
        }
    }
    public override void ExitState(EnemyStateManager enemy)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.enemyCollider.enabled = false;
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.enemyCollider.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGStateManager : EnemyStateManager
{
    public PGBarrelSummonState poisonState = new PGBarrelSummonState();
    public float nextTimeToThrowPoison = 0f;
    protected override void OnEnable()
    {
        normalState = new PGNormalState();
        skillState = new PGSkillState();
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Start()
    {
        base.Start();
        nextTimeToThrowPoison = 0f;
    }
    protected override void Update()
    {
        base.Update();
    }
    public void ThrowPoison()
    {
        aimPoint = target.position;

        // Summon barrel
        Weapon weapon = WeaponDatabase.weaponList[10];
        weapon.weaponBaseEffect.weaponPoint = enemyShootingPoint;
        weapon.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, false, null, rb, ref spawnedBullet);
    }
}

public class PGNormalState : EnemyNormalState
{
    PGStateManager PGstate;
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
        PGstate = enemy.GetComponent<PGStateManager>();
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
            if (enemy.enemyStat.enemyBehavior == "Patrol" && enemy.provoked)
            {
                enemy.rb.velocity = moveDir * enemy.enemyStat.enemyMovementSpeed * 2f;
            }
            else
            {
                enemy.rb.velocity = moveDir * enemy.enemyStat.enemyMovementSpeed;
            }
            if (moveDir.x > 0.1f)
            {
                enemy.enemySprite.transform.localScale = new Vector3(Mathf.Abs(enemy.enemySprite.transform.localScale.x), enemy.enemySprite.transform.localScale.y, enemy.enemySprite.transform.localScale.z);
            }
            else if (moveDir.x < -0.1f)
            {
                enemy.enemySprite.transform.localScale = new Vector3(-Mathf.Abs(enemy.enemySprite.transform.localScale.x), enemy.enemySprite.transform.localScale.y, enemy.enemySprite.transform.localScale.z);
            }

            float distanceToNextWayPoint = Vector2.Distance(enemy.scanPoint.position, enemy.path.vectorPath[enemy.currentWayPoint]);
            float distanceToTarget = Vector2.Distance(enemy.scanPoint.position, enemy.target.position);

            if (Time.time > PGstate.nextTimeToThrowPoison)
            {
                enemy.SwitchState(PGstate.poisonState);
            }
            else if (distanceToTarget < enemy.enemyStat.enemyAtkRange[0] && Time.time > enemy.nextTimeToUseSkill && enemy.provoked)
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
        base.ExitState(enemy);
    }
}

public class PGSkillState : EnemySkillState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Here");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        base.UpdateState(enemy);
    }
    public override void ExitState(EnemyStateManager enemy)
    {
        base.ExitState(enemy);
    }
}

public class PGBarrelSummonState : EnemyBaseState
{
    PGStateManager PGstate;
    public override void EnterState(EnemyStateManager enemy)
    {
        PGstate = enemy.GetComponent<PGStateManager>();
        enemy.rb.velocity = Vector2.zero;
        enemy.animator.SetTrigger("Throw");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        PGstate.nextTimeToThrowPoison = Time.time + 2.5f;
    }
}
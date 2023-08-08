using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KFStateManager : EnemyStateManager
{
    public KFBarrelSummonState barrelState = new KFBarrelSummonState();
    public float nextTimeToSummonBarrel = 0f;
    public GameObject[] barrel;
    protected override void OnEnable()
    {
        normalState = new KFNormalState();
        skillState = new KFSkillState();
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Start()
    {
        base.Start();
        nextTimeToSummonBarrel = 0f;
    }
    protected override void Update()
    {
        base.Update();
    }
    public void SummonBarrel()
    {
        Vector3 aim = target.position;
        rb.velocity = Vector2.zero;

        // Summon barrel
        aimPoint = target.position + Vector3.up * enemyShootingPoint.localPosition.y;
        animator.SetTrigger("Shoot");
        Vector3 dir = ((Vector2)aim - (Vector2)transform.position).normalized;
        Vector3[] spawnPos = { (Vector2)transform.position + (Vector2)dir ,
        (Vector2)transform.position + ((Vector2)dir + (Vector2)Vector3.Cross(Vector3.forward, dir)).normalized,
        (Vector2)transform.position + ((Vector2)dir - (Vector2)Vector3.Cross(Vector3.forward, dir)).normalized};

        for (int i = 0; i < spawnPos.Length; i++)
        {
            if (!Physics2D.OverlapCircle(spawnPos[i], 0.25f, LayerMask.GetMask("Wall", "Obstacle")))
            {
                GameObject.Instantiate(barrel[Random.Range(0, barrel.Length)]).transform.position = spawnPos[i];
            }
        }
    }
}

public class KFNormalState : EnemyNormalState
{
    KFStateManager KFstate;
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
        KFstate = enemy.GetComponent<KFStateManager>();
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

            Collider2D result = Physics2D.OverlapCircle(enemy.scanPoint.position, 0.75f, LayerMask.GetMask("Obstacle"));
            if (result != null)
            {
                enemy.skillState.fixedTarget = result.transform;
                enemy.SwitchState(enemy.skillState);
            }
            else if (Time.time > KFstate.nextTimeToSummonBarrel
                && !Physics2D.BoxCast(enemy.scanPoint.position, Vector2.one * 0.25f, 0f, (enemy.target.position - enemy.scanPoint.position).normalized, (enemy.target.position - enemy.scanPoint.position).magnitude, LayerMask.GetMask("Wall")))
            {
                enemy.SwitchState(KFstate.barrelState);
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

public class KFSkillState : EnemySkillState
{
    KFStateManager KFstate;
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
        KFstate = enemy.GetComponent<KFStateManager>();
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

public class KFBarrelSummonState : EnemyBaseState
{
    KFStateManager KFstate;
    public override void EnterState(EnemyStateManager enemy)
    {
        KFstate = enemy.GetComponent<KFStateManager>();
        enemy.animator.SetTrigger("Summon");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        KFstate.nextTimeToSummonBarrel = Time.time + 3.25f;
    }
}
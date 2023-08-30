using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KataStateManager : EnemyStateManager
{
    public KataSlideState slidingState = new KataSlideState();
    public KataWideSlashState slashingState = new KataWideSlashState();
    public KataShockWaveState shockWaveState = new KataShockWaveState();
    public KataGrenadeThrowingState throwingState = new KataGrenadeThrowingState();

    public Weapon currentWeapon;
    public IEnumerator aimingCourotine;

    public float nextTimeToSlide = 0f;
    public float nextTimeToSlash = 0f;
    public float nextTimeToShockWave = 0f;
    public float nextTimeToThrowGrenade = 0f;

    protected override void OnEnable()
    {
        normalState = new KataNormalState();
        skillState = new KataSkillState();
        base.OnEnable();
        aimingCourotine = UpdateAim(0f);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected IEnumerator UsingWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        weapon.weaponBaseEffect.weaponPoint = enemyShootingPoint;
        while (true)
        {
            weapon.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, false, null, rb, ref spawnedBullet);
            yield return new WaitForSeconds(4f / weapon.atkSpd);
        }
    }
    public override void StopSkill()
    {
        StopCoroutine(aimingCourotine);
        StopCoroutine(shootingCoroutine);
        if (currentState == stunState)
            return;
        if (currentWeapon != null)
        {
            currentWeapon.weaponBaseEffect.Release(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - currentWeapon.accuracy) / 100f, false, null, ref spawnedBullet);
            currentWeapon = null;
        }
    }
    public void SlideSlash()
    {
        StopSkill();
        shootingCoroutine = UsingWeapon(WeaponDatabase.weaponList[12]);
        aimingCourotine = UpdateAim(1f);
        StartCoroutine(aimingCourotine);
        StartCoroutine(shootingCoroutine);
    }
    public void WideSlash()
    {
        StopSkill();
        shootingCoroutine = UsingWeapon(WeaponDatabase.kataWeaponList[0]);
        aimingCourotine = UpdateAim(0.1f);
        StartCoroutine(aimingCourotine);
        StartCoroutine(shootingCoroutine);
    }
    public void ShockWave()
    {
        StopSkill();

        if (currentHP > enemyStat.enemyMaxHP / 2f)
            shootingCoroutine = UsingWeapon(WeaponDatabase.kataWeaponList[1]);
        else
            shootingCoroutine = UsingWeapon(WeaponDatabase.kataWeaponList[2]);

        aimingCourotine = UpdateAim(0.2f);
        StartCoroutine(aimingCourotine);
        StartCoroutine(shootingCoroutine);
    }
    public void ThrowGrenade()
    {
        StopSkill();
        shootingCoroutine = UsingWeapon(WeaponDatabase.weaponList[4]);
        aimingCourotine = UpdateAim(0f);
        StartCoroutine(aimingCourotine);
        StartCoroutine(shootingCoroutine);
    }

    public IEnumerator UpdateAim(float interval)
    {
        while (true)
        {
            aimPoint = target.position + new Vector3(0f, 0.5f, 0f);
            if (aimPoint.x - transform.position.x > 0.1f)
            {
                enemySprite.transform.localScale = new Vector3(Mathf.Abs(enemySprite.transform.localScale.x), enemySprite.transform.localScale.y, enemySprite.transform.localScale.z);
            }
            else if (aimPoint.x - transform.position.x < -0.1f)
            {
                enemySprite.transform.localScale = new Vector3(-Mathf.Abs(enemySprite.transform.localScale.x), enemySprite.transform.localScale.y, enemySprite.transform.localScale.z);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}

public class KataNormalState : EnemyNormalState
{
    KataStateManager kataState;
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
        kataState = enemy.GetComponent<KataStateManager>();
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

            if(Time.time > enemy.nextTimeToUseSkill && distanceToTarget <= 8f)
            {
                if(kataState.slidingState.counter > 0)
                {
                    kataState.SwitchState(kataState.slidingState);
                }
                else if (Time.time > kataState.nextTimeToThrowGrenade && enemy.currentHP <= enemy.enemyStat.enemyMaxHP / 2f && Random.Range(0, 100) < 25)
                {
                    kataState.SwitchState(kataState.throwingState);
                }
                else if (Time.time > kataState.nextTimeToSlash && Vector2.Distance(enemy.target.position, enemy.transform.position) <= 3f)
                {
                    kataState.SwitchState(kataState.slashingState);
                }
                else if (Time.time > kataState.nextTimeToShockWave && (Random.Range(0, 100) < 20
                    || Physics2D.CapsuleCast(enemy.enemyShootingPoint.position, new Vector2(0.65f, 1.25f), CapsuleDirection2D.Vertical, 0f, (enemy.target.position - enemy.enemyShootingPoint.position).normalized, (enemy.target.position - enemy.enemyShootingPoint.position).magnitude, LayerMask.GetMask("Wall"))))
                {
                    kataState.SwitchState(kataState.shockWaveState);
                }
                else if (Time.time > kataState.nextTimeToSlide)
                {
                    kataState.slidingState.counter = 0;
                    kataState.SwitchState(kataState.slidingState);
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

public class KataSkillState : EnemySkillState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
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

public class KataGrenadeThrowingState : EnemyBaseState
{
    KataStateManager kataState;
    public override void EnterState(EnemyStateManager enemy)
    {
        kataState = enemy.GetComponent<KataStateManager>();
        enemy.rb.velocity = Vector2.zero;
        enemy.animator.SetTrigger("Throw");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        kataState.nextTimeToThrowGrenade = Time.time + 2f;
        kataState.nextTimeToUseSkill = 0.5f;
    }
}
public class KataSlideState : EnemyBaseState
{
    KataStateManager kataState;
    public int counter = 0;
    public override void EnterState(EnemyStateManager enemy)
    {
        kataState = enemy.GetComponent<KataStateManager>();
        enemy.rb.velocity = Vector2.zero;
        if (counter == 0)
            enemy.animator.SetTrigger("Slide");
        else
            enemy.animator.SetTrigger("SecondSlide");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        if (enemy.currentHP > enemy.enemyStat.enemyMaxHP / 2f || counter > 0)
        {
            kataState.nextTimeToSlide = Time.time + 1f;
            kataState.nextTimeToUseSkill = 0.5f;
            counter = 0;
        }
        else
        {
            counter++;
        }
    }
}
public class KataWideSlashState : EnemyBaseState
{
    KataStateManager kataState;
    public override void EnterState(EnemyStateManager enemy)
    {
        kataState = enemy.GetComponent<KataStateManager>();
        enemy.rb.velocity = Vector2.zero;
        enemy.animator.SetTrigger("WideSlash");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        kataState.nextTimeToSlash = Time.time + 0.5f;
        kataState.nextTimeToUseSkill = 0.5f;
    }
}
public class KataShockWaveState : EnemyBaseState
{
    KataStateManager kataState;
    public override void EnterState(EnemyStateManager enemy)
    {
        kataState = enemy.GetComponent<KataStateManager>();
        enemy.rb.velocity = Vector2.zero;
        enemy.animator.SetTrigger("ShockWave");
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        kataState.nextTimeToShockWave = Time.time + 3f;
        kataState.nextTimeToUseSkill = 0.5f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;
    public EnemySpawnState spawnState = new EnemySpawnState();
    public EnemyNormalState normalState = new EnemyNormalState();
    public EnemySkillState skillState = new EnemySkillState();
    public EnemyStunState stunState = new EnemyStunState();
    public EnemyDeadState deadState = new EnemyDeadState();

    public Rigidbody2D rb;
    public Collider2D enemyCollider;
    public Animator animator;
    public SpriteRenderer enemySprite;

    [Header("Enemy Status")]
    public int currentHP;
    public bool marked;
    public bool provoked;
    public bool isInvincible;
    public bool isPossessed;
    public bool isExploding;
    public float burnTime = 0;
    public float burnTickTime = 0;

    [Header("Enemy Skill")]
    public Transform enemyShootingPoint;
    public float nextTimeToUseSkill;
    public GameObject spawnedBullet;
    public IEnumerator shootingCoroutine;

    [Header("EnemyUI")]
    public Slider HPSlider;
    public GameObject mark;
    public GameObject burningVFX;

    [Header("Sprite Effect")]
    public Material whiteFlashMat;
    [System.NonSerialized] public Material defaultMat;
    private float flashWhiteTimer = 0;
    private float damagedAnimationTimer = 0;
    private bool show;

    [Header("Universal Stuffs")]
    public EnemyStat enemyStat;
    public PlayerStat playerStat;
    public EventBroadcast eventBroadcast;

    [Header("Pathfinder")]
    public Seeker seeker;
    public Transform target;
    public Transform scanPoint;
    public Vector3 aimPoint;
    public LayerMask blockMask;
    public float nextWayPointDistance = 2f;
    public Transform[] patrolPath;
    private int currentPathIndex = 0;
    [System.NonSerialized] public Path path;
    [System.NonSerialized] public Path tempPath;
    [System.NonSerialized] public int currentWayPoint;
    [System.NonSerialized] public bool isEndOfPath;
    public IEnumerator updatePathCourotine;

    protected virtual void OnEnable()
    {
        updatePathCourotine = UpdatePath();
        shootingCoroutine = Shooting();
    }
    protected virtual void OnDisable()
    {
        StopCoroutine(updatePathCourotine);
    }
    protected virtual void Start()
    {
        // Stat
        currentHP = enemyStat.enemyMaxHP;
        HPSlider.maxValue = enemyStat.enemyMaxHP;
        HPSlider.value = HPSlider.maxValue;
        marked = false;
        burningVFX.SetActive(false);
        switch (enemyStat.enemyBehavior)
        {
            case "Patrol":
            case "Passive":
                provoked = false;
                break;
            default:
                provoked = true;
                break;
        }
        isInvincible = false;
        isPossessed = false;
        isExploding = false;
        currentPathIndex = 0;
        nextTimeToUseSkill = Time.time + enemyStat.enemyCD[0];

        //Animation
        defaultMat = enemySprite.material;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        StartCoroutine(updatePathCourotine);
        SwitchState(spawnState);
    }
    protected virtual void Update()
    {
        if (Time.time - damagedAnimationTimer < 0.6f && currentState != deadState)
        {
            if (Time.time - flashWhiteTimer >= 0.2f)
            {
                if (show)
                {
                    enemySprite.material = defaultMat;
                }
                else
                {
                    enemySprite.material = whiteFlashMat;
                }
                show = !show;
                flashWhiteTimer = Time.time;
            }
        }
        else if (currentState != deadState && currentState != stunState)
        {
            enemySprite.material = defaultMat;
        }
        if (burnTime > Time.time)
        {
            if (Time.time > burnTickTime)
            {
                TakeDamage(Mathf.FloorToInt(enemyStat.enemyMaxHP / 100f));
                burnTickTime = Time.time + 0.5f;
            }
        }
        else if (burningVFX.activeSelf)
        {
            burningVFX.SetActive(false);
        }
        currentState.UpdateState(this);
    }
    public void SwitchState(EnemyBaseState state)
    {
        if (currentState != null)
            currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;
        if (damage > 0)
        {
            provoked = true;
            animator.SetBool("Provoked", true);
        }
        damagedAnimationTimer = Time.time;
        currentHP = Mathf.Min(enemyStat.enemyMaxHP, currentHP - damage);
        HPSlider.value = currentHP;

        if (currentHP <= 0)
        {
            SwitchState(deadState);
        }
    }
    #region status effect
    public void GetStun(float duration, bool resetAtk)
    {
        stunState.stunDuration = duration;
        stunState.resetAtk = resetAtk;
        SwitchState(stunState);
    }
    public void GetBurn(float duration)
    {
        burnTime = Time.time + duration;
        burningVFX.SetActive(true);
    }
    #endregion
    public void UsingSkill()
    {
        StartCoroutine(shootingCoroutine);
    }
    public void StopSkill()
    {
        StopCoroutine(shootingCoroutine);
        Weapon weapon = WeaponDatabase.weaponList[enemyStat.enemyWeaponId[0]];
        weapon.weaponBaseEffect.Release(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, false, null, ref spawnedBullet);
    }
    public void BackToNormal()
    {
        if (currentState != deadState)
            SwitchState(normalState);
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            tempPath = p;
            if (path == null)
            {
                path = tempPath;
                currentWayPoint = 0;
            }
            else
            {
                bool condition;
                switch (enemyStat.enemyBehavior)
                {
                    case "Flee":
                        condition = (tempPath.GetTotalLength() > path.GetTotalLength())
                    || Vector2.Distance(tempPath.vectorPath[tempPath.vectorPath.Count - 1], path.vectorPath[path.vectorPath.Count - 1]) > enemyStat.enemyAtkRange[0] * 2f;
                        break;
                    default:
                        condition = (tempPath.GetTotalLength() < path.GetTotalLength())
                    || Vector2.Distance(tempPath.vectorPath[tempPath.vectorPath.Count - 1], path.vectorPath[path.vectorPath.Count - 1]) > enemyStat.enemyAtkRange[0] * 2f;
                        break;
                }
                if (condition)
                {
                    path = tempPath;
                    currentWayPoint = 0;
                }
            }

        }
    }
    public void FinishPossessionAnimation()
    {
        eventBroadcast.FinishPossessionAnimationNoti();
    }
    public void FinishExplodingAnimation()
    {
        if (marked && !isPossessed)
        {
            WeaponDatabase.fishingMail.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, enemyShootingPoint.position, false, playerStat, ref spawnedBullet);
        }
        eventBroadcast.EnemyKilledNoti();
        if (Random.Range(0, 100) > 75)
        {
            playerStat.money += 10;
            eventBroadcast.UpdateMoneyNoti();
        }
        if (enemyStat.enemyType == "Miniboss")
            eventBroadcast.FinishStageNoti();
        Destroy(gameObject);
    }
    protected virtual IEnumerator Shooting()
    {
        while (true)
        {
            Weapon weapon = WeaponDatabase.weaponList[enemyStat.enemyWeaponId[0]];
            weapon.weaponBaseEffect.weaponPoint = enemyShootingPoint;
            weapon.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, false, null, ref spawnedBullet);
            yield return new WaitForSeconds(5f / weapon.atkSpd);
        }
    }
    protected virtual IEnumerator UpdatePath()
    {
        while (true)
        {
            bool fail = false;
            if (seeker.IsDone())
            {
                Vector3 temp;
                switch (enemyStat.enemyBehavior)
                {
                    case "Melee":
                        seeker.StartPath(scanPoint.position, target.position + Random.insideUnitSphere * enemyStat.enemyAtkRange[0] * 0.5f, OnPathComplete);
                        break;
                    case "Hold":
                        temp = target.position + Random.onUnitSphere * enemyStat.enemyAtkRange[0] * 0.5f;
                        if (!enemyStat.requireLOS[0] || (enemyStat.requireLOS[0] && !Physics2D.Linecast(temp, target.position, LayerMask.GetMask("Wall"))))
                        {
                            seeker.StartPath(scanPoint.position, temp, OnPathComplete);
                        }
                        else
                            fail = true;
                        break;
                    case "Flee":
                        Vector3 dir = (scanPoint.position - target.position).normalized;
                        Vector3 rotation = Quaternion.Euler(new Vector3(0f,0f,Random.Range(-60,60))) * dir;
                        seeker.StartPath(scanPoint.position, target.position + rotation * enemyStat.enemyAtkRange[0] * 0.75f, OnPathComplete);
                        break;
                    case "Patrol":
                        if (!provoked)
                        {
                            if (Vector2.Distance(scanPoint.position, patrolPath[currentPathIndex].position) < 1f)
                            {
                                currentPathIndex = (currentPathIndex + 1) % patrolPath.Length;
                            }
                            else
                            {
                                seeker.StartPath(scanPoint.position, patrolPath[currentPathIndex].position, OnPathComplete);
                            }
                        }
                        else
                        {
                            temp = target.position + Random.insideUnitSphere * enemyStat.enemyAtkRange[0];
                            if (!enemyStat.requireLOS[0] || (enemyStat.requireLOS[0] && !Physics2D.Linecast(temp, target.position, LayerMask.GetMask("Wall"))))
                            {
                                seeker.StartPath(scanPoint.position, temp, OnPathComplete);
                            }
                            else
                                fail = true;
                        }
                        break;
                    case "Passive":
                        if (provoked)
                        {
                            temp = target.position + Random.insideUnitSphere * enemyStat.enemyAtkRange[0];
                            if (!enemyStat.requireLOS[0] || (enemyStat.requireLOS[0] && !Physics2D.Linecast(temp, target.position, LayerMask.GetMask("Wall"))))
                            {
                                seeker.StartPath(scanPoint.position, temp, OnPathComplete);
                            }
                            else
                                fail = true;
                        }
                        break;
                    default:
                        temp = target.position + Random.insideUnitSphere * enemyStat.enemyAtkRange[0];
                        if (!enemyStat.requireLOS[0] || (enemyStat.requireLOS[0] && !Physics2D.Linecast(temp, target.position, LayerMask.GetMask("Wall"))))
                        {
                            seeker.StartPath(scanPoint.position, temp, OnPathComplete);
                        }
                        else
                            fail = true;
                        break;
                }
            }
            if (!fail)
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForSeconds(0.1f);
        }
    }

}
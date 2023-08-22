using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using TMPro;

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
    public bool isGolden;
    public float spawnTime;
    public bool isInvincible;
    public bool isPossessed;
    public bool isExploding;
    public int burnStack = 0;
    public float burnTime = 0;
    public float burnTickTime = 0;
    public int poisonStack = 0;
    public float poisonTime = 0;
    public float poisonTickTime = 0;
    public float speedModifier = 0;
    private IEnumerator speedCoroutine;
    public GameObject beingControlledBy;

    [Header("Enemy Skill")]
    public Transform enemyShootingPoint;
    public float nextTimeToUseSkill;
    public GameObject spawnedBullet;
    public IEnumerator shootingCoroutine;

    [Header("EnemyUI")]
    public Slider HPSlider;
    public Image HPFill;
    public TextMeshProUGUI statusText;
    public GameObject mark;
    public GameObject burningVFX;
    public GameObject poisonVFX;
    public GameObject healingVFX;
    public GameObject speedUpVFX;
    public GameObject speedDownVFX;

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
        if (Random.Range(0, 100) >= 95 && enemyStat.enemyType == "Default")
        {
            isGolden = true;
            HPFill.color = new Color32(206, 206, 107, 255);
        }
        else
        {
            isGolden = false;
        }
        spawnTime = Time.time;
        eventBroadcast.sendingCard += StopBug;
    }
    protected virtual void OnDisable()
    {
        eventBroadcast.sendingCard -= StopBug;
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
        healingVFX.SetActive(false);
        speedUpVFX.SetActive(false);
        speedUpVFX.SetActive(false);
        speedModifier = 0;
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
        nextTimeToUseSkill = Time.time + enemyStat.enemyCD[0] * 0.25f;

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

        #region status update
        string status = "";
        if (burnTime > Time.time)
        {
            if (status != "")
                status += " ";
            status += "<sprite=\"StatusEffect\" index=0> x" + burnStack.ToString();
            if (Time.time > burnTickTime)
            {
                TakeDamage(burnStack);
                burnTickTime = Time.time + 0.5f;
            }
        }
        else if (burningVFX.activeSelf)
        {
            burnStack = 0;
            burningVFX.SetActive(false);
        }
        if (poisonTime > Time.time)
        {
            if(status != "")
                status += " ";
            status += "<sprite=\"StatusEffect\" index=1> x" + poisonStack.ToString();
            if (Time.time > poisonTickTime)
            {
                Debug.Log(poisonStack);
                TakeDamage(poisonStack);
                poisonTickTime = Time.time + 0.2f;
            }
        }
        else if (poisonVFX.activeSelf)
        {
            poisonStack = 0;
            poisonVFX.SetActive(false);
        }
        statusText.text = status;
        #endregion

        if (isGolden && Time.time - spawnTime > 15f && currentState != deadState)
        {
            if (marked)
            {
                StopBug();
                WeaponDatabase.fishingMail.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, enemyShootingPoint.position, false, playerStat, rb, ref spawnedBullet);
            }
            TakeDamage(99999);
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
        if (isInvincible || currentState == deadState)
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
    public void StopBug()
    {
        marked = false;
        mark.SetActive(false);
    }
    #region status effect
    public void GetStun(float duration, bool resetAtk)
    {
        if (currentState == deadState)
            return;
        stunState.stunDuration = duration;
        stunState.resetAtk = resetAtk;
        SwitchState(stunState);
    }
    public void GetBurn(int stack = 1)
    {
        burnStack = Mathf.Min(10, burnStack + stack);
        burnTime = Time.time + 5f;
        burningVFX.SetActive(true);
    }
    public void GetPoison(int stack = 1)
    {
        if(Random.Range(0,100) > enemyStat.poisonImmunity)
        {
            poisonStack = Mathf.Min(5, poisonStack + stack);
            poisonTime = Time.time + 2.5f;
            poisonVFX.SetActive(false);
            poisonVFX.SetActive(true);
        }
    }
    public void GetSpeedChange(float ammount, float duration)
    {
        if(speedCoroutine != null)
            StopCoroutine(speedCoroutine);
        speedCoroutine = SpeedChange(ammount, duration);
        StartCoroutine(speedCoroutine);
    }
    IEnumerator SpeedChange(float ammount, float duration)
    {
        if(ammount > 0)
        {
            speedUpVFX.SetActive(false);
            speedDownVFX.SetActive(false);
            speedUpVFX.SetActive(true);
            speedModifier = ammount;
        }
        else
        {
            speedUpVFX.SetActive(false);
            speedDownVFX.SetActive(false);
            speedDownVFX.SetActive(true);
            speedModifier = ammount;
        }
        yield return new WaitForSeconds(duration);
        speedUpVFX.SetActive(false);
        speedDownVFX.SetActive(false);
        speedModifier = 0;
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
                    case "Random":
                        condition = false;
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
            WeaponDatabase.fishingMail.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, enemyShootingPoint.position, false, playerStat, rb, ref spawnedBullet);
        }
        eventBroadcast.EnemyKilledNoti();
        if (isPossessed && isGolden)
        {
            playerStat.money += 50;
            eventBroadcast.UpdateMoneyNoti();
        }
        if (Random.Range(0, 100) >= 75)
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
            weapon.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, false, null, rb, ref spawnedBullet);
            yield return new WaitForSeconds(4f / weapon.atkSpd);
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
                    case "Station":
                        seeker.StartPath(scanPoint.position, scanPoint.position, OnPathComplete);
                        break;
                    case "Melee":
                        seeker.StartPath(scanPoint.position, target.position + Random.insideUnitSphere * enemyStat.enemyAtkRange[0] * 0.25f, OnPathComplete);
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
                        Vector3 rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-60, 60))) * dir;
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
                    case "Random":
                        seeker.StartPath(scanPoint.position, scanPoint.position + Random.onUnitSphere * 3f, OnPathComplete);
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
                yield return new WaitForSeconds(0.25f);
            else
                yield return new WaitForSeconds(0.1f);
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class EnemyStateManager : MonoBehaviour
{
    private EnemyBaseState currentState;
    public EnemyNormalState normalState = new EnemyNormalState();
    public EnemySkillState skillState = new EnemySkillState();
    public EnemyStuntState stuntState = new EnemyStuntState();
    public EnemyDeadState deadState = new EnemyDeadState();

    public Rigidbody2D rb;
    public Collider2D enemyCollider;
    public Animator animator;
    public SpriteRenderer enemySprite;
    public GameObject possessionCollider;

    [Header("Enemy Status")]
    public int currentHP;
    public bool marked;
    public bool isInvincible;
    public bool isPossessed;
    public bool isExploding;

    [Header("Enemy Skill")]
    public Transform enemyShootingPoint;
    public float nextTimeToUseSkill;
    public IEnumerator shootingCoroutine;

    [Header("EnemyUI")]
    public Slider HPSlider;
    public GameObject mark;

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
    [System.NonSerialized] public Path path;
    [System.NonSerialized] public int currentWayPoint;
    [System.NonSerialized] public bool isEndOfPath;
    private IEnumerator updatePathCourotine;

    private void OnEnable()
    {
        updatePathCourotine = UpdatePath();
        shootingCoroutine = Shooting();
    }
    private void OnDisable()
    {
        StopCoroutine(updatePathCourotine);
    }
    private void Start()
    {
        possessionCollider.SetActive(false);

        // Stat
        currentHP = enemyStat.enemyMaxHP;
        HPSlider.maxValue = enemyStat.enemyMaxHP;
        HPSlider.value = HPSlider.maxValue;
        marked = false;
        isInvincible = false;
        isPossessed = false;
        isExploding = false;
        nextTimeToUseSkill = Time.time + enemyStat.enemyCD[0];

        //Animation
        defaultMat = enemySprite.material;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        StartCoroutine(updatePathCourotine);
        SwitchState(normalState);
    }
    private void Update()
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
        else if(currentState != deadState && currentState != stuntState)
        {
            enemySprite.material = defaultMat;
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

        damagedAnimationTimer = Time.time;
        currentHP = Mathf.Min(enemyStat.enemyMaxHP, currentHP - damage);
        HPSlider.value = currentHP;

        if (currentHP <= 0)
        {
            SwitchState(deadState);
        }
    }
    public void GetStunt(float duration)
    {
        stuntState.stuntDuration = duration;
        SwitchState(stuntState);
    }
    public void UsingSkill()
    {
        StartCoroutine(shootingCoroutine);
    }
    public void StopSkill()
    {
        StopCoroutine(shootingCoroutine);
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
            path = p;
            currentWayPoint = 0;
        }
    }
    public void FinishPossessionAnimation()
    {
        possessionCollider.SetActive(false);
        eventBroadcast.FinishPossessionAnimationNoti();
    }
    public void FinishExplodingAnimation()
    {
        if (marked && !isPossessed)
        {
            WeaponDatabase.fishingMail.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, enemyShootingPoint.position, false, playerStat);
        }
        eventBroadcast.EnemyKilledNoti();
        if (Random.Range(0, 100) > 75)
        {
            playerStat.money += 10;
            eventBroadcast.UpdateMoneyNoti();
        }
        Destroy(gameObject);
    }
    IEnumerator Shooting()
    {
        while (true)
        {
            Weapon weapon = WeaponDatabase.weaponList[enemyStat.enemyWeaponId[0]];
            weapon.weaponBaseEffect.ApplyEffect(enemyShootingPoint.position, (Vector2)aimPoint + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, false, null);
            yield return new WaitForSeconds(5f / weapon.atkSpd);
        }
    }
    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(scanPoint.position, target.position + Random.insideUnitSphere * enemyStat.enemyAtkRange[0]*0.5f, OnPathComplete);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

}
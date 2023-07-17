using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerBaseState currentState;
    public PlayerNormalState normalState = new PlayerNormalState();
    public PlayerDashState dashState = new PlayerDashState();
    public PlayerFlinchState flinchState = new PlayerFlinchState();
    public PlayerPossessState possessState = new PlayerPossessState();

    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer[] playerSprites;
    public Transform weaponPivotPoint;
    public SpriteRenderer weaponSprite;
    public WeaponTrigger trigger;
    public GameObject shockWave;

    [Header("Status")]
    public int currentHP;
    public float speedPenalty;
    public bool isInvincible = false;


    [Header("Animator")]
    public Animator bodyAnimator;
    public Animator handAnimator;
    public Animator weaponAnimator;
    public OffHand offHand;
    public Slider HPSlider;
    private float damagedAnimationTimer;
    private float flashWhiteTimer;
    private bool show;
    public Material whiteFlashMat;
    public Material defaultMat;

    [Header("Dashing")]
    public int dashNumber = 0;
    public float nextDashResetTime = 0f;

    [Header("Universal Stuffs")]
    public PlayerStat playerStat;
    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        UpdateWeaponSprite();

        eventBroadcast.finishPossessionAnimation += FinishPossessAnimation;
        eventBroadcast.finishPossessionAnimation += possessState.Teleport;
    }

    private void OnDisable()
    {
        eventBroadcast.finishPossessionAnimation -= FinishPossessAnimation;
        eventBroadcast.finishPossessionAnimation -= possessState.Teleport;
    }

    private void Start()
    {
        // Status
        HPSlider.maxValue = playerStat.maxHP;
        currentHP = playerStat.currentHP;
        HPSlider.value = currentHP;
        isInvincible = false;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        defaultMat = playerSprites[0].material;
        shockWave.SetActive(false);
        SwitchState(normalState);
    }
    private void Update()
    {
        if(Time.time > nextDashResetTime && dashNumber != 0)
        {
            dashNumber = 0;
        }
        if (Time.time - damagedAnimationTimer < 0.6f)
        {
            if (Time.time - flashWhiteTimer >= 0.1f)
            {
                if (show)
                {
                    foreach(SpriteRenderer sr in playerSprites)
                    {
                        sr.material = defaultMat;
                    }
                }
                else
                {
                    foreach (SpriteRenderer sr in playerSprites)
                    {
                        sr.material = whiteFlashMat;
                    }
                }
                show = !show;
                flashWhiteTimer = Time.time;
            }
        }
        else
        {
            foreach (SpriteRenderer sr in playerSprites)
            {
                sr.material = defaultMat;
            }
        }
        currentState.UpdateState(this);
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, 0.5f, 0f), 2.5f);
    }
    public void SwitchState(PlayerBaseState state)
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
        currentHP = Mathf.Min(playerStat.maxHP, currentHP - damage);
        HPSlider.value = currentHP;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateWeaponSprite()
    {
        weaponSprite.sprite = WeaponDatabase.weaponList[playerStat.currentWeapon[0]].weaponSprite;
    }

    public void FinishPossessAnimation()
    {
        if(currentState == possessState)
            SwitchState(normalState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerBaseState currentState;
    public PlayerSpawnState spawnState = new PlayerSpawnState();
    public PlayerNormalState normalState = new PlayerNormalState();
    public PlayerDashState dashState = new PlayerDashState();
    public PlayerSpawnState flinchState = new PlayerSpawnState();
    public PlayerPossessState possessState = new PlayerPossessState();
    public PlayerTeleportState teleportState = new PlayerTeleportState();
    public PlayerStunState stunState = new PlayerStunState();

    public Rigidbody2D rb;
    public Collider2D hurtBoxCol;
    public SpriteRenderer[] playerSprites;
    public Transform weaponPivotPoint;
    public SpriteRenderer weaponSprite;
    public Image subWeaponSprite;
    public Slider switchCooldownSlider;
    public WeaponTrigger trigger;
    public GameObject shockWave;
    public GameObject map;
    public GameObject upgradeSelection;

    [Header("Status")]
    public float speedPenalty;
    public bool isInvincible = false;
    public bool isInUI = false;

    [Header("Interact")]
    public List<GameObject> interactableObj = new List<GameObject>();
    public GameObject interactingObj;

    [Header("Animator")]
    public Animator bodyAnimator;
    public Animator handAnimator;
    public Animator weaponAnimator;
    public GameObject LevelUpVFX;
    public OffHand offHand;
    public Slider LvlSlider;
    public TextMeshProUGUI LvlText;
    public TextMeshProUGUI ammo;
    private float damagedAnimationTimer;
    private float flashWhiteTimer;
    private bool show;
    public Material whiteFlashMat;
    public Material defaultMat;
    public GameObject healingVFX;

    [Header("Dashing")]
    public int dashNumber = 0;
    public float nextDashResetTime = 0f;

    [Header("Register")]
    public GameObject canPossessMarker;
    [System.NonSerialized] public GameObject spawnedBullet;
    [System.NonSerialized] public EnemyStateManager enemy;

    [Header("Universal Stuffs")]
    public PlayerStat playerStat;
    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        UpdateWeaponSprite();
        isInUI = false;
        eventBroadcast.enterUI += EnterUI;
        eventBroadcast.exitUI += ExitUI;
        eventBroadcast.allDead += GainEXP;
        eventBroadcast.possessEvent += CanPossess;
        eventBroadcast.finishPossessionAnimation += FinishPossessAnimation;
        eventBroadcast.finishPossessionAnimation += possessState.Teleport;
    }

    private void OnDisable()
    {
        eventBroadcast.enterUI -= EnterUI;
        eventBroadcast.exitUI -= ExitUI;
        eventBroadcast.allDead -= GainEXP;
        eventBroadcast.possessEvent -= CanPossess;
        eventBroadcast.finishPossessionAnimation -= FinishPossessAnimation;
        eventBroadcast.finishPossessionAnimation -= possessState.Teleport;
    }

    private void Start()
    {
        // Status
        playerStat.hasCard = true;
        eventBroadcast.UpdateHPNoti();
        eventBroadcast.UpdateMoneyNoti();
        LvlText.text = playerStat.level.ToString();
        LvlSlider.maxValue = playerStat.level;
        LvlSlider.value = playerStat.exp - (playerStat.level * (playerStat.level - 1)) / 2;
        ammo.text = ((playerStat.currentAmmo[0] >= 0) ? playerStat.currentAmmo[0].ToString() : "Åá") + "|" + ((playerStat.currentAmmo[1] >= 0) ? playerStat.currentAmmo[1].ToString() : "Åá");
        switchCooldownSlider.value = 1;
        isInvincible = false;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        defaultMat = playerSprites[0].material;
        shockWave.SetActive(false);
        SwitchState(spawnState);
    }
    private void Update()
    {
        if (Time.time > nextDashResetTime && dashNumber != 0)
        {
            dashNumber = 0;
        }
        if (Time.time - damagedAnimationTimer < 0.6f)
        {
            if (Time.time - flashWhiteTimer >= 0.1f)
            {
                if (show)
                {
                    foreach (SpriteRenderer sr in playerSprites)
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
    public void SwitchState(PlayerBaseState state)
    {
        if (currentState != null)
            currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    #region health
    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        damagedAnimationTimer = Time.time;
        if(damage > 0)
            playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP - Mathf.CeilToInt(damage * (100 - playerStat.defPerc) / 100f));
        else
        {
            playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP - damage);
            healingVFX.SetActive(false);
            healingVFX.SetActive(true);
        }
        eventBroadcast.UpdateHPNoti();

        if (playerStat.currentHP <= 0)
        {
            hurtBoxCol.enabled = false;
            eventBroadcast.GenerateMapNoti();
            SceneManager.LoadScene("Home");
        }
    }
    #endregion

    #region Weapon Sprite
    public void UpdateWeaponSprite()
    {
        weaponSprite.sprite = WeaponDatabase.weaponList[playerStat.currentWeapon[0]].weaponSprite;
        subWeaponSprite.sprite = WeaponDatabase.weaponList[playerStat.currentWeapon[1]].weaponSprite;
        ammo.text = ((playerStat.currentAmmo[0] >= 0) ? playerStat.currentAmmo[0].ToString() : "Åá") + "|" + ((playerStat.currentAmmo[1] >= 0) ? playerStat.currentAmmo[1].ToString() : "Åá");
    }
    #endregion

    #region possession related
    public void CanPossess(EnemyStateManager enemy)
    {
        canPossessMarker.SetActive(true);
        this.enemy = enemy;
        StartCoroutine(CannotPossess());
    }
    public void FinishPossessAnimation()
    {
        if (currentState == possessState)
        {
            enemy = null;
            SwitchState(normalState);
        }
    }
    public void TeleportToNextStage()
    {
        Portal portal = interactingObj.GetComponent<Portal>();
        MapGenerator.Instance.Travel(portal.nextRoomType, portal.nextRoomIndex);
    }
    IEnumerator CannotPossess()
    {
        yield return new WaitForSeconds(1.5f);
        enemy = null;
        canPossessMarker.SetActive(false);
    }
    #endregion

    #region UI
    private void GainEXP()
    {
        playerStat.exp++;
        while (playerStat.exp >= (playerStat.level * (playerStat.level + 1)) / 2)
        {
            playerStat.level++;
            playerStat.luck += 10;
            LevelUpVFX.SetActive(false);
            LevelUpVFX.SetActive(true);
            upgradeSelection.SetActive(true);
        }
        LvlText.text = playerStat.level.ToString();
        LvlSlider.maxValue = playerStat.level;
        LvlSlider.value = playerStat.exp - (playerStat.level * (playerStat.level - 1)) / 2;
    }
    private void EnterUI()
    {
        isInUI = true;
    }
    private void ExitUI()
    {
        isInUI = false;
    }
    #endregion

    #region animation
    public void FinishSpawning()
    {
        if(currentState == spawnState)
            SwitchState(normalState);
    }
    #endregion

    public void GetStun(float duration)
    {
        stunState.stunDuration = duration;
        SwitchState(stunState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region highlight focus obj
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interact"))
        {
            interactableObj.Add(collision.gameObject);
            if (interactableObj.Count > 1)
            {
                TurnOffHighlight(interactableObj[interactableObj.Count - 2]);
            }
            TurnOnHighlight(interactableObj[interactableObj.Count - 1]);
        }
        #endregion
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interact"))
        {
            TurnOffHighlight(interactableObj[interactableObj.Count - 1]);
            interactableObj.Remove(collision.gameObject);
            if (interactableObj.Count > 0)
            {
                TurnOnHighlight(interactableObj[interactableObj.Count - 1]);
            }
        }
    }

    private void TurnOnHighlight(GameObject gObj)
    {
        switch (gObj.tag)
        {
            case "Portal":
                gObj.GetComponent<Portal>().TurnOnHighlight();
                break;
            case "NPC":
                gObj.GetComponent<NPC>().TurnOnHighlight();
                break;
            case "OneTime":
                gObj.GetComponent<Onetime>().TurnOnHighlight();
                break;
        }
    }
    private void TurnOffHighlight(GameObject gObj)
    {
        switch (gObj.tag)
        {
            case "Portal":
                gObj.GetComponent<Portal>().TurnOffHighlight();
                break;
            case "NPC":
                gObj.GetComponent<NPC>().TurnOffHighlight();
                break;
            case "OneTime":
                gObj.GetComponent<Onetime>().TurnOffHighlight();
                break;
        }
    }

    private static class CoroutineUtil
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
}

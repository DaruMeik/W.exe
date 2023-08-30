using System;
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
    public PlayerDeadState deadState = new PlayerDeadState();

    public Rigidbody2D rb;
    public Collider2D hurtBoxCol;
    public SpriteRenderer[] playerSprites;
    public Transform weaponPivotPoint;
    public SpriteRenderer weaponSprite;
    public WeaponTrigger trigger;
    public GameObject shockWave;
    public GameObject map;
    public GameObject upgradeSelection;

    [Header("Status")]
    public float speedPenalty;
    public bool isInvincible = false;
    public bool isInUI = false;
    public IEnumerator IFrameCoroutine;
    public int burnStack = 0;
    public float burnTime = 0;
    public float burnTickTime = 0;
    public int poisonStack = 0;
    public float poisonTime = 0;
    public float poisonTickTime = 0;
    public float speedModifier = 0f;
    private int hitTaken = 0;
    private IEnumerator speedCoroutine;
    private IEnumerator defCoroutine;
    public IEnumerator possessCoroutine;
    public GameObject beingControlledBy;

    private Queue<string> queue = new Queue<string>();


    [Header("Interact")]
    public List<GameObject> interactableObj = new List<GameObject>();
    public GameObject interactingObj;

    [Header("Animator")]
    public Animator bodyAnimator;
    public Animator handAnimator;
    public Animator weaponAnimator;
    public GameObject LevelUpVFX;
    public OffHand offHand;
    private float damagedAnimationTimer;
    private float flashWhiteTimer;
    private bool show;
    public Material whiteFlashMat;
    public Material defaultMat;

    [Header("UI")]
    public TextMeshProUGUI statusText;
    public GameObject afterImageVFX;
    public GameObject burningVFX;
    public GameObject poisonVFX;
    public GameObject healingVFX;
    public GameObject speedUpVFX;
    public GameObject speedDownVFX;
    public GameObject defUpVFX;
    public GameObject defDownVFX;

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
        if (playerStat.wildCard)
        {
            StartCoroutine(WildShuffle());
        }
        bodyAnimator.updateMode = AnimatorUpdateMode.Normal;
        UpdateWeaponSprite();
        isInUI = false;
        eventBroadcast.updateWeaponSprite += UpdateWeaponSprite;
        eventBroadcast.healVFX += PlayHealingVFX;
        eventBroadcast.enterUI += EnterUI;
        eventBroadcast.exitUI += ExitUI;
        eventBroadcast.gainExp += GainExpQueue;
        eventBroadcast.possessEvent += CanPossess;
        eventBroadcast.finishPossessionAnimation += possessState.Teleport;
        eventBroadcast.finishPossessionAnimation += FinishPossessAnimation;
    }

    private void OnDisable()
    {
        eventBroadcast.updateWeaponSprite -= UpdateWeaponSprite;
        eventBroadcast.healVFX -= PlayHealingVFX;
        eventBroadcast.enterUI -= EnterUI;
        eventBroadcast.exitUI -= ExitUI;
        eventBroadcast.gainExp -= GainExpQueue;
        eventBroadcast.possessEvent -= CanPossess;
        eventBroadcast.finishPossessionAnimation -= possessState.Teleport;
        eventBroadcast.finishPossessionAnimation -= FinishPossessAnimation;
    }

    private void Start()
    {
        // Status
        eventBroadcast.UpdateHPNoti();
        eventBroadcast.UpdateCardUINoti();
        eventBroadcast.UpdateLvlNoti();
        eventBroadcast.UpdateMoneyNoti();
        eventBroadcast.UpdateWeaponNoti();
        playerStat.atkPerc = playerStat.defaultAtkPerc;
        playerStat.defPerc = playerStat.defaultDefPerc;
        isInvincible = false;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        hitTaken = 0;
        defaultMat = playerSprites[0].material;
        healingVFX.SetActive(false);
        burningVFX.SetActive(false);
        speedUpVFX.SetActive(false);
        speedDownVFX.SetActive(false);
        defUpVFX.SetActive(false);
        defDownVFX.SetActive(false);
        SwitchState(spawnState);
    }
    private void Update()
    {
        if (Time.time > nextDashResetTime && dashNumber != 0)
        {
            dashNumber = 0;
        }
        if (currentState != deadState)
        {
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
        }

        #region status
        string status = "";
        if (burnTime > Time.time)
        {
            if (status != "")
                status += " ";
            status += "<sprite=\"StatusEffect\" index=0> x" + burnStack.ToString();
            if (Time.time > burnTickTime)
            {
                TakeDamage(burnStack, true);
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
            if (status != "")
                status += " ";
            status += "<sprite=\"StatusEffect\" index=1> x" + poisonStack.ToString();
            if (Time.time > poisonTickTime)
            {
                TakeDamage(poisonStack, true);
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

        #region exp queue
        if(queue.Count > 0 && !isInUI)
        {
            switch (queue.Dequeue())
            {
                case "Red":
                    GainRedExp(1);
                    break;
                case "Green":
                    GainGreenExp(1);
                    break;
                case "Blue":
                    GainBlueExp(1);
                    break;
            }
        }
        #endregion

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
    public void TakeDamage(int damage, bool byEnvironment = false)
    {
        if (isInvincible && !byEnvironment)
            return;

        if (damage > 0)
        {
            hitTaken++;
            if (playerStat.shockArmor && hitTaken % 5 == 0)
            {
                GameObject.Instantiate(shockWave, transform);
            }
            if (playerStat.rage)
                playerStat.atkPerc += 2;
            damagedAnimationTimer = Time.time;
            if (playerStat.curseOfDefense == 0)
                playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP - Mathf.CeilToInt(damage * (100 - playerStat.defPerc) / 100f));
            else
                playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP - Mathf.CeilToInt(damage * (100 + 10 - playerStat.defPerc) / 100f));
        }
        else if (damage < 0)
        {
            eventBroadcast.HealVFXNoti();
            playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP - damage);
        }
        eventBroadcast.UpdateHPNoti();

        if (playerStat.currentHP <= 0)
        {
            SwitchState(deadState);
        }
    }
    public void PlayHealingVFX()
    {
        healingVFX.SetActive(false);
        healingVFX.SetActive(true);
    }
    public void SelfDestruct()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            playerStat.ResetStat();
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            eventBroadcast.GenerateMapNoti();
            SceneManager.LoadScene("Home");
        }
    }
    #endregion

    #region Weapon Sprite
    public void UpdateWeaponSprite()
    {
        weaponSprite.sprite = WeaponDatabase.weaponList[playerStat.currentWeapon[playerStat.currentIndex]].weaponSprite;
        eventBroadcast.UpdateWeaponNoti();
    }
    #endregion

    #region possession related
    public void CanPossess(EnemyStateManager enemy)
    {
        canPossessMarker.SetActive(true);
        this.enemy = enemy;
        possessCoroutine = CannotPossess();
        StartCoroutine(possessCoroutine);
    }
    public void FinishPossessAnimation()
    {
        if (currentState == possessState)
        {
            enemy = null;
            if (IFrameCoroutine != null)
                StopCoroutine(IFrameCoroutine);
            IFrameCoroutine = InvulnerableFrame();
            StartCoroutine(IFrameCoroutine);
            SwitchState(normalState);
        }
    }
    IEnumerator CannotPossess()
    {
        yield return new WaitForSeconds(2.5f);
        enemy = null;
        canPossessMarker.SetActive(false);
    }
    public IEnumerator InvulnerableFrame()
    {
        yield return new WaitForSeconds(0.6f);
        hurtBoxCol.enabled = true;
        IFrameCoroutine = null;
    }
    #endregion

    public void TeleportToNextStage()
    {
        if (interactingObj.name == "Tutorial")
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (interactingObj.name == "Home")
        {
            playerStat.ResetStat();
            SceneManager.LoadScene("Home");
        }
        else
        {
            Portal portal = interactingObj.GetComponent<Portal>();
            MapGenerator.Instance.Travel(portal.nextRoomType, portal.nextRoomIndex);
        }
    }

    #region UI
    private void GainExpQueue(int amount, string type)
    {
        for(int i = 0; i < amount; i++)
        {
            queue.Enqueue(type);
        }
    }
    private void GainRedExp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerStat.redExp++;
            LevelUpVFX.SetActive(false);
            LevelUpVFX.SetActive(true);
            if (playerStat.redExp >= (playerStat.redLevel * (playerStat.redLevel + 1)) / 2)
            {
                playerStat.redLevel++;
                upgradeSelection.GetComponent<UpgradeSelection>().rewardType = "Red";
                upgradeSelection.SetActive(true);
            }
            eventBroadcast.UpdateLvlNoti();
        }
    }
    private void GainGreenExp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerStat.greenExp++;
            LevelUpVFX.SetActive(false);
            LevelUpVFX.SetActive(true);
            if (playerStat.greenExp >= (playerStat.greenLevel * (playerStat.greenLevel + 1)) / 2)
            {
                playerStat.greenLevel++;
                upgradeSelection.GetComponent<UpgradeSelection>().rewardType = "Green";
                upgradeSelection.SetActive(true);
            }
            eventBroadcast.UpdateLvlNoti();
        }
    }
    private void GainBlueExp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerStat.blueExp++;
            LevelUpVFX.SetActive(false);
            LevelUpVFX.SetActive(true);
            if (playerStat.blueExp >= (playerStat.blueLevel * (playerStat.blueLevel + 1)) / 2)
            {
                playerStat.blueLevel++;
                upgradeSelection.GetComponent<UpgradeSelection>().rewardType = "Blue";
                upgradeSelection.SetActive(true);
            }
            eventBroadcast.UpdateLvlNoti();
        }
    }
    private void EnterUI()
    {
        rb.velocity = Vector2.zero;
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
        if (currentState == spawnState)
            SwitchState(normalState);
    }
    #endregion

    #region status effect
    public void GetStun(float duration)
    {
        stunState.stunDuration = duration;
        SwitchState(stunState);
    }
    public void GetBurn(int stack)
    {
        burnStack = Mathf.Min(10, burnStack + stack);
        if (playerStat.fireResistance)
            burnTime = Time.time + 2.5f;
        else
            burnTime = Time.time + 5f;
        burningVFX.SetActive(true);
    }
    public void GetPoison(int stack = 1)
    {
        Debug.Log(poisonStack);
        poisonStack = Mathf.Min(5, poisonStack + stack);
        if (playerStat.poisonResistance)
            poisonTime = Time.time + 1.25f;
        else
            poisonTime = Time.time + 2.5f;
        poisonVFX.SetActive(false);
        poisonVFX.SetActive(true);
    }
    public void GetSpeedChange(float amount, float duration)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);
        speedCoroutine = SpeedChange(amount, duration);
        StartCoroutine(speedCoroutine);
    }
    IEnumerator SpeedChange(float amount, float duration)
    {
        if (amount > 0)
        {
            speedUpVFX.SetActive(false);
            speedDownVFX.SetActive(false);
            speedUpVFX.SetActive(true);
            speedModifier = amount;
        }
        else
        {
            speedUpVFX.SetActive(false);
            speedDownVFX.SetActive(false);
            speedDownVFX.SetActive(true);
            speedModifier = amount;
        }
        yield return new WaitForSeconds(duration);
        speedUpVFX.SetActive(false);
        speedDownVFX.SetActive(false);
        speedModifier = 0;
    }
    public void GetDefChange(float amount, float duration)
    {
        if (defCoroutine != null)
            StopCoroutine(defCoroutine);
        defCoroutine = DefChange(amount, duration);
        StartCoroutine(defCoroutine);
    }
    IEnumerator DefChange(float amount, float duration)
    {
        if (amount > 0)
        {
            defUpVFX.SetActive(false);
            defDownVFX.SetActive(false);
            defUpVFX.SetActive(true);
        }
        else
        {
            defUpVFX.SetActive(false);
            defDownVFX.SetActive(false);
            defDownVFX.SetActive(true);
        }
        playerStat.defPerc = playerStat.defaultDefPerc + (int)amount;
        yield return new WaitForSeconds(duration);
        defUpVFX.SetActive(false);
        defDownVFX.SetActive(false);
        playerStat.defPerc = playerStat.defaultDefPerc;
    }
    #endregion

    #region wild card
    IEnumerator WildShuffle()
    {
        while (true)
        {
            do
            {
                playerStat.currentWeapon[0] = UnityEngine.Random.Range(0, WeaponDatabase.weaponList.Count);
            }
            while (playerStat.unsellableWeapon.Contains(playerStat.currentWeapon[0]));
            float ammoModifier = 0f;
            switch (WeaponDatabase.weaponList[playerStat.currentWeapon[0]].weaponType)
            {
                case "Gun":
                    if (playerStat.fireBullet)
                    {
                        ammoModifier += 200;
                    }
                    else if (playerStat.sharpBullet)
                    {
                        ammoModifier += 200;
                    }
                    break;
                case "Melee":
                    if (playerStat.unseenBlade)
                    {
                        ammoModifier += 200;
                    }
                    if (playerStat.reflectSword)
                    {
                        ammoModifier += 200;
                    }
                    break;
                case "Charge":
                    if (playerStat.fasterCharge)
                    {
                        ammoModifier += 200;
                    }
                    break;
                case "Special":
                    if (playerStat.sturdyBuild)
                    {
                        ammoModifier += 200;
                    }
                    if (playerStat.goldBuild)
                    {
                        ammoModifier += 200;
                    }
                    break;
            }
            playerStat.currentAmmo[0]
                = Mathf.CeilToInt(WeaponDatabase.weaponList[playerStat.currentWeapon[0]].maxAmmo
                * (100 + ammoModifier) / 100f);


            do
            {
                playerStat.currentWeapon[1] = UnityEngine.Random.Range(0, WeaponDatabase.weaponList.Count);
            }
            while (playerStat.unsellableWeapon.Contains(playerStat.currentWeapon[1]));
            ammoModifier = 0f;
            switch (WeaponDatabase.weaponList[playerStat.currentWeapon[1]].weaponType)
            {
                case "Gun":
                    if (playerStat.fireBullet)
                    {
                        ammoModifier += 200;
                    }
                    else if (playerStat.sharpBullet)
                    {
                        ammoModifier += 200;
                    }
                    break;
                case "Melee":
                    if (playerStat.unseenBlade)
                    {
                        ammoModifier += 200;
                    }
                    if (playerStat.reflectSword)
                    {
                        ammoModifier += 200;
                    }
                    break;
                case "Charge":
                    if (playerStat.fasterCharge)
                    {
                        ammoModifier += 200;
                    }
                    break;
                case "Special":
                    if (playerStat.sturdyBuild)
                    {
                        ammoModifier += 200;
                    }
                    if (playerStat.goldBuild)
                    {
                        ammoModifier += 200;
                    }
                    break;
            }
            playerStat.currentAmmo[1]
                = Mathf.CeilToInt(WeaponDatabase.weaponList[playerStat.currentWeapon[1]].maxAmmo
                * (100 + ammoModifier) / 100f);

            normalState.nextTimeToShoot1 = Time.time + 1f;
            normalState.nextTimeToShoot2 = Time.time + 1f;
            normalState.isShooting1 = false;
            normalState.isShooting2 = false;

            eventBroadcast.UpdateWeaponNoti();
            UpdateWeaponSprite();

            yield return new WaitForSeconds(10f);
        }
    }
    #endregion

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

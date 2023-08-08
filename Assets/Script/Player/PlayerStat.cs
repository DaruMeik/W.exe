using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObj/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    // General Status
    public int currentHP;
    public int currentIndex;
    public int[] currentWeapon;
    public int[] currentAmmo;
    public bool hasCard;
    public int level;
    public int exp;

    [Header("Default Stat")]
    public int defaultMaxHP;
    public float defaultPlayerSpeed;
    public int defaultMaxDash;
    public float defaultShockWaveRange;
    public float defaultShockWaveStunTime;
    public int defaultMoney;
    public int defaultLuck;
    public int defaultAtkPerc;
    public int defaultDefPerc;

    [Header("Stat")]
    public int defaultWeapon = 0;
    public int maxHP;
    public float playerSpeed;
    public int maxDash;
    public float shockWaveRange;
    public float shockWaveStunTime;
    public int money;
    public int luck;
    public int atkPerc;
    public int defPerc;

    [Header("BodyUpgrade")]
    public int extraCardDamage;
    public int extraAmmoPerc;
    public int extraPossessHealingPerc;
    public bool shockwaveDealDamage;
    public bool cardShockWave;

    [Header("WeaponUpgrade")]
    public bool burningBullet;

    // Regis stuffs
    public List<int> levelUpgradeRegister = new List<int>();
    public List<int> stageUpgradeRegister = new List<int>();


    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        defaultMaxHP = 100;
        defaultPlayerSpeed = 5f;
        defaultMaxDash = 2;
        defaultShockWaveRange = 2.5f;
        defaultShockWaveStunTime = 0.5f;
        defaultMoney = 0;
        defaultLuck = -20;
        defaultAtkPerc = 0;
        defaultDefPerc = 0;
        level = 1;
        exp = 0;

        defaultWeapon = 0;
        maxHP = defaultMaxHP;
        playerSpeed = defaultPlayerSpeed;
        maxDash = defaultMaxDash;
        shockWaveRange = defaultShockWaveRange;
        shockWaveStunTime = defaultShockWaveStunTime;
        money = defaultMoney;
        luck = defaultLuck;
        atkPerc = defaultAtkPerc;
        defPerc = defaultDefPerc;
        currentHP = maxHP;

        // Body Upgrade
        extraCardDamage = 0;
        extraAmmoPerc = 0;
        extraPossessHealingPerc = 0;
        shockwaveDealDamage = false;
        cardShockWave = false;

        // Weapon Upgrade
        burningBullet = false;

        currentIndex = 0;
        currentWeapon = new int[2] { 0, 0 };
        currentAmmo = new int[2] { -1, -1 };
        hasCard = true;
        levelUpgradeRegister.Clear();
        stageUpgradeRegister.Clear();
    }
    public void ResetStat()
    {
        level = 1;
        exp = 0;

        hasCard = true;
        maxHP = defaultMaxHP;
        playerSpeed = defaultPlayerSpeed;
        maxDash = defaultMaxDash;
        shockWaveRange = defaultShockWaveRange;
        shockWaveStunTime = defaultShockWaveStunTime;
        money = defaultMoney;
        luck = defaultLuck;
        atkPerc = defaultAtkPerc;
        defPerc = defaultDefPerc;

        // Body Upgrade
        extraCardDamage = 0;
        extraAmmoPerc = 0;
        extraPossessHealingPerc = 0;
        shockwaveDealDamage = false;
        cardShockWave = false;

        // Weapon Upgrade
        burningBullet = false;

        defaultWeapon = 0;
        currentHP = maxHP;
        currentIndex = 0;
        currentWeapon = new int[2] { 0, 0 };
        currentAmmo = new int[2] { -1, -1 };
        levelUpgradeRegister.Clear();
        stageUpgradeRegister.Clear();
        eventBroadcast.UpdateWeaponSpriteNoti();
        eventBroadcast.UpdateCardUINoti();
        Time.timeScale = 1f;
    }
    public void UpdateCard(bool hasCard)
    {
        this.hasCard = hasCard;
        eventBroadcast.UpdateCardUINoti();
    }
}

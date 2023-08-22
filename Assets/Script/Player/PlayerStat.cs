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
    public int cardReadyPerc;
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
    public int defaultWeaponAtkUpPerc = 0;
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
    public bool fireBullet;
    public bool fasterCharge;
    public bool unseenBlade;
    public bool BEEGBomb;
    public bool critableGun;
    public bool reflectSword;
    public bool sharpBullet;
    public bool wildCard;
    public bool sturdyBuild;
    public bool goldBuild;

    // Regis stuffs
    public List<int> unsellableWeapon = new List<int>();
    public List<int> levelUpgradeRegister = new List<int>();
    public List<int> bulletModRegister = new List<int>();


    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        defaultMaxHP = 200;
        defaultPlayerSpeed = 5f;
        defaultMaxDash = 1;
        defaultShockWaveRange = 2.5f;
        defaultShockWaveStunTime = 0.5f;
        defaultMoney = 0;
        defaultLuck = -20;
        defaultAtkPerc = 0;
        defaultDefPerc = 0;
        ResetStat();
    }
    public void ResetStat()
    {
        level = 1;
        exp = 0;

        cardReadyPerc = 100;
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
        fireBullet = false;
        fasterCharge = false;
        unseenBlade = false;
        BEEGBomb = false;
        critableGun = false;
        reflectSword = false;
        sharpBullet = false;
        wildCard = false;
        sturdyBuild = false;
        goldBuild = false;

        defaultWeapon = 0;
        defaultWeaponAtkUpPerc = 0;
        currentHP = maxHP;
        currentIndex = 0;
        currentWeapon = new int[2] { 11, 0 };
        currentAmmo = new int[2] { -1, -1 };
        unsellableWeapon = new List<int> { 0, 6, 9 };
        levelUpgradeRegister.Clear();
        bulletModRegister.Clear();
        eventBroadcast.UpdateWeaponSpriteNoti();
        eventBroadcast.UpdateCardUINoti();
        Time.timeScale = 1f;
    }
}

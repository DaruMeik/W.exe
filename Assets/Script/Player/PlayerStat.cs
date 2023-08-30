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
    public int redLevel;
    public int redExp;
    public int greenLevel;
    public int greenExp;
    public int blueLevel;
    public int blueExp;

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
    public int gem;
    public int chip;
    public int luck;
    public int atkPerc;
    public int defPerc;

    [Header("RedUpgrade")]
    public int extraCardDamage;
    public int extraAmmoPerc;
    public bool shockwaveDealDamage;
    public bool cardShockWave;
    public bool rage;
    public bool closeCombat;
    public bool sharpShooter;
    public bool stubborn;

    [Header("GreenUpgrade")]
    public int extraAtkSpeedPerc;
    public bool swiftMovement;
    public bool featherStep;
    public bool shadowMovement;
    
    [Header("BlueUpgrade")]
    public int extraPossessHealingPerc;
    public bool movingFort;
    public bool poisonResistance;
    public bool fireResistance;
    public bool shockArmor;
    public bool shockBlast;

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

    [Header("Curse")]
    public int curseOfOffense;
    public int curseOfDefense;
    public int curseOfMobility;

    // Regis stuffs
    public List<int[]> weaponUpgradeRegi = new List<int[]>();
    public List<int> unsellableWeapon = new List<int>();
    public List<int> redUpgradeRegister = new List<int>();
    public List<int> greenUpgradeRegister = new List<int>();
    public List<int> blueUpgradeRegister = new List<int>();
    public List<int> bulletModRegister = new List<int>();


    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        defaultMaxHP = 150;
        defaultPlayerSpeed = 4.5f;
        defaultMaxDash = 1;
        defaultShockWaveRange = 2.5f;
        defaultShockWaveStunTime = 0.5f;
        defaultMoney = 0;
        defaultLuck = -20;
        defaultAtkPerc = 0;
        defaultDefPerc = 0;
        gem = 0;
        chip = 0;

        weaponUpgradeRegi.Clear();
        foreach(Weapon weapon in WeaponDatabase.weaponList)
        {
            weaponUpgradeRegi.Add(new int[2] { weapon.id, 0 });
        }

        ResetStat();

        eventBroadcast.allDead += ReduceCurseCounter;
    }
    private void OnDisable()
    {
        eventBroadcast.allDead -= ReduceCurseCounter;
    }
    public void ResetStat()
    {
        redLevel = 1;
        redExp = 0;
        greenLevel = 1;
        greenExp = 0;
        blueLevel = 1;
        blueExp = 0;

        defaultAtkPerc = 0;
        defaultDefPerc = 0;

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

        // Red Upgrade
        extraCardDamage = 0;
        extraAmmoPerc = 0;
        shockwaveDealDamage = false;
        cardShockWave = false;
        rage = false;
        closeCombat = false;
        sharpShooter = false;
        stubborn = false;

        // Green Upgrade
        extraAtkSpeedPerc = 0;
        swiftMovement = false;
        featherStep = false;
        shadowMovement = false;

        // Blue Upgrade
        extraPossessHealingPerc = 0;
        movingFort = false;
        poisonResistance = false;
        fireResistance = false;
        shockArmor = false;
        shockBlast = false;

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

        // Curse
        curseOfOffense = 0;
        curseOfDefense = 0;
        curseOfMobility = 0;

        defaultWeapon = 0;
        defaultWeaponAtkUpPerc = 0;
        currentHP = maxHP;
        currentIndex = 0;
        currentWeapon = new int[2] { 0, 0 };
        currentAmmo = new int[2] { -1, -1 };
        unsellableWeapon = new List<int> { 0, 6, 9, 12 };
        redUpgradeRegister.Clear();
        greenUpgradeRegister.Clear();
        blueUpgradeRegister.Clear();
        bulletModRegister.Clear();
        eventBroadcast.UpdateWeaponSpriteNoti();
        eventBroadcast.UpdateCardUINoti();
        Time.timeScale = 1f;
    }

    private void ReduceCurseCounter()
    {
        curseOfOffense = Mathf.Max(0, curseOfOffense - 1);
        curseOfDefense = Mathf.Max(0, curseOfDefense - 1);
        curseOfMobility = Mathf.Max(0, curseOfMobility - 1);
    }
}

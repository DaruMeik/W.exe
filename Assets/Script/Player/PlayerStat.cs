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
    public int maxHP;
    public float playerSpeed;
    public int maxDash;
    public float shockWaveRange;
    public float shockWaveStunTime;
    public int money;
    public int luck;
    public int atkPerc;
    public int defPerc;

    [Header("Upgrade")]
    public int extraCardDamage;
    public int extraAmmoPerc;
    public int extraPossessHealingPerc;
    public bool shockwaveDealDamage;
    public bool cardShockWave;

    // Regis stuffs
    public List<int> upgradeRegister = new List<int>();


    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        defaultMaxHP = 100;
        defaultPlayerSpeed = 5f;
        defaultMaxDash = 2;
        defaultShockWaveRange = 2.5f;
        defaultShockWaveStunTime = 0.5f;
        defaultMoney = 0;
        defaultLuck = -5;
        defaultAtkPerc = 0;
        defaultDefPerc = 0;
        level = 1;
        exp = 0;

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

        extraCardDamage = 0;
        extraAmmoPerc = 0;
        extraPossessHealingPerc = 0;
        shockwaveDealDamage = false;
        cardShockWave = false;


        currentIndex = 0;
        currentWeapon = new int[2] { 0, 0 };
        currentAmmo = new int[2] { -1, -1 };
        hasCard = true;
        upgradeRegister.Clear();
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

        extraCardDamage = 0;
        extraAmmoPerc = 0;
        extraPossessHealingPerc = 0;
        shockwaveDealDamage = false;
        cardShockWave = false;

        currentHP = maxHP;
        currentIndex = 0;
        currentWeapon = new int[2] { 0, 0 };
        currentAmmo = new int[2] { -1, -1 };
        upgradeRegister.Clear();
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

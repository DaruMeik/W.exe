using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObj/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    // General Status
    public int currentHP;
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
        currentWeapon = new int[2] { 5, 0 };
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


        currentHP = maxHP;
        currentWeapon = new int[2] { 2, 0 };
        currentAmmo = new int[2] { -1, -1 };
        upgradeRegister.Clear();
    }
    public void UpdateCard(bool hasCard)
    {
        this.hasCard = hasCard;
        eventBroadcast.UpdateCardUINoti();
    }
}

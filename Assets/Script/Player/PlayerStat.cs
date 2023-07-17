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

    // Stat
    public int maxHP;
    public float playerSpeed;
    public int maxDash;
    public float shockWaveRange;
    public float shockWaveStuntTime;

    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        maxHP = 100;
        playerSpeed = 5f;
        maxDash = 2;
        shockWaveRange = 2.5f;
        shockWaveStuntTime = 0.5f;

        currentHP = 100;
        currentWeapon = new int[2] { 2, 0 };
        currentAmmo = new int[2] { -1, 0 };
        hasCard = true;
    }

    public void UpdateCard(bool hasCard)
    {
        this.hasCard = hasCard;
        eventBroadcast.UpdateCardUINoti();
    }
}

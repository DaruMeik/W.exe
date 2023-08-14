using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP + Mathf.FloorToInt(playerStat.maxHP * 25f / 100f));
        eventBroadcast.HealVFXNoti();
        playerStat.eventBroadcast.UpdateHPNoti();
        Destroy(gameObject);
    }
}

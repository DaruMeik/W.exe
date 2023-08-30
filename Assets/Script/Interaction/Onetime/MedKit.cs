using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP + 50);
        eventBroadcast.HealVFXNoti();
        playerStat.eventBroadcast.UpdateHPNoti();
        Destroy(gameObject);
    }
}

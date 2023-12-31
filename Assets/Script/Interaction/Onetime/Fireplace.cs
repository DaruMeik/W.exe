using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : Onetime
{
    [SerializeField] Animator animator;
    public override void Interact()
    {
        if (hasBeenUsed)
            return;
        hasBeenUsed = true;
        animator.SetTrigger("Use");
        playerStat.currentHP = Mathf.Min(playerStat.maxHP, playerStat.currentHP + Mathf.FloorToInt(playerStat.maxHP * 50f/100f));
        eventBroadcast.HealVFXNoti();
        playerStat.eventBroadcast.UpdateHPNoti();
        triggerCol.enabled = false;
        TurnOffHighlight();
    }
}

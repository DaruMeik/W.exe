using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeKit : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.maxHP += 25;
        playerStat.eventBroadcast.UpdateHPNoti();
        Destroy(gameObject);
    }
}

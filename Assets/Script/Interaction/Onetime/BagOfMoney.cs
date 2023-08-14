using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagOfMoney : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.money += 20;
        eventBroadcast.UpdateMoneyNoti();
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeirdMachine : Onetime
{
    public override void Interact()
    {
        if (hasBeenUsed)
            return;
        hasBeenUsed = true;
        int buff = Random.Range(0, 3);
        int nerf = Random.Range(0, 3);
        switch (buff)
        {
            case 0:
                playerStat.atkPerc += 10;
                break;
            case 1:
                playerStat.defPerc += 10;
                break;
            case 2:
                playerStat.luck += 10;
                break;
        }
        switch (nerf)
        {
            case 0:
                playerStat.atkPerc -= 10;
                break;
            case 1:
                playerStat.defPerc -= 10;
                break;
            case 2:
                playerStat.luck -= 10;
                break;
        }
        eventBroadcast.FinishNewMachineNoti(buff, nerf);
        triggerCol.enabled = false;
        TurnOffHighlight();
    }
}

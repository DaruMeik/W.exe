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
        int buffIndex = Random.Range(0, 3);
        for(int i = 0; i < 3; i++)
        {
            if(i != buffIndex)
            {
                switch (i)
                {
                    case 0:
                        eventBroadcast.GainExpNoti(1, "Red");
                        break;
                    case 1:
                        eventBroadcast.GainExpNoti(1, "Green");
                        break;
                    case 2:
                        eventBroadcast.GainExpNoti(1, "Blue");
                        break;
                }
            }
        }
        int curesIndex = Random.Range(0, 3);
        switch (curesIndex)
        {
            case 0:
                playerStat.curseOfOffense = 3;
                break;
            case 1:
                playerStat.curseOfDefense = 3;
                break;
            case 2:
                playerStat.curseOfMobility = 3;
                break;
        }
        eventBroadcast.FinishNewMachineNoti(curesIndex);
        triggerCol.enabled = false;
        TurnOffHighlight();
    }
}

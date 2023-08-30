using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.chip += 1;
        Destroy(gameObject);
    }
}

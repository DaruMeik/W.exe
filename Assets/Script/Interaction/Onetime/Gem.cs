using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.gem += 1;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Onetime
{
    public override void Interact()
    {
        hasBeenUsed = true;
        playerStat.currentAmmo[0] += WeaponDatabase.weaponList[playerStat.currentWeapon[0]].maxAmmo;
        playerStat.currentAmmo[1] += WeaponDatabase.weaponList[playerStat.currentWeapon[1]].maxAmmo;
        eventBroadcast.UpdateWeaponNoti();
        Destroy(gameObject);
    }
}

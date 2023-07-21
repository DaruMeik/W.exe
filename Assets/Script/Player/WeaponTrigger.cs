using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public Weapon weapon;
    public PlayerStateManager player;
    public Vector2 aimPos;
    public void Trigger()
    {
        weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, aimPos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat);
        player.playerStat.currentAmmo[0]--;
        player.ammo.text = ((player.playerStat.currentAmmo[0] >= 0) ? player.playerStat.currentAmmo[0].ToString() : "Åá") + "|"
            + ((player.playerStat.currentAmmo[1] >= 0) ? player.playerStat.currentAmmo[1].ToString() : "Åá");
        if (player.playerStat.currentAmmo[0] == 0)
        {
            player.playerStat.currentWeapon[0] = 0;
            player.playerStat.currentAmmo[0] = -1;
            player.ammo.text = ((player.playerStat.currentAmmo[0] >= 0) ? player.playerStat.currentAmmo[0].ToString() : "Åá") + "|"
                + ((player.playerStat.currentAmmo[1] >= 0) ? player.playerStat.currentAmmo[1].ToString() : "Åá");
            player.UpdateWeaponSprite();
            player.normalState.isShooting = false;
        }
    }
}

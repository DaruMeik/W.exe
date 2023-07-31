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
        weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, aimPos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, ref player.spawnedBullet);
        player.playerStat.currentAmmo[player.playerStat.currentIndex]--;
        player.eventBroadcast.UpdateWeaponNoti();
        if (player.playerStat.currentAmmo[player.playerStat.currentIndex] == 0)
        {
            player.playerStat.currentWeapon[player.playerStat.currentIndex] = 0;
            player.playerStat.currentAmmo[player.playerStat.currentIndex] = -1;
            player.eventBroadcast.UpdateWeaponNoti();
            player.UpdateWeaponSprite();
            player.normalState.isShooting = false;
        }
    }
}

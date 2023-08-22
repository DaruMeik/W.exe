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
        weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, aimPos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, player.rb, ref player.spawnedBullet);
        if (player.playerStat.currentAmmo[player.playerStat.currentIndex] > 0) 
            player.playerStat.currentAmmo[player.playerStat.currentIndex]--;
        player.eventBroadcast.UpdateWeaponNoti();
        if (player.playerStat.currentAmmo[player.playerStat.currentIndex] == 0)
        {
            switch (player.playerStat.currentIndex)
            {
                case 0:
                    player.normalState.nextTimeToShoot1 = Time.time + 1f;
                    player.normalState.isShooting1 = false;
                    break;
                case 1:
                    player.normalState.nextTimeToShoot2 = Time.time + 1f;
                    player.normalState.isShooting2 = false;
                    break;
            }
            player.playerStat.currentWeapon[player.playerStat.currentIndex] = player.playerStat.defaultWeapon;
            player.playerStat.currentAmmo[player.playerStat.currentIndex] = -1;
            player.eventBroadcast.UpdateWeaponNoti();
            player.UpdateWeaponSprite();
        }
    }
}

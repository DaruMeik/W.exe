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
    }
}

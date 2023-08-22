using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHand : MonoBehaviour
{
    public PlayerStateManager player;
    public Vector2 mousePos;
    private void ThrowCard()
    {
        WeaponBaseEffect temp = WeaponDatabase.fishingMail.weaponBaseEffect;
        temp.weaponPoint = player.transform;
        temp.ApplyEffect(player.weaponPivotPoint.position, mousePos, true, player.playerStat, player.rb, ref player.spawnedBullet);
    }
}

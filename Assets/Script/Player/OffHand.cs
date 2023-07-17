using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHand : MonoBehaviour
{
    public PlayerStateManager player;
    public Vector2 mousePos;
    private void ThrowCard()
    {
        WeaponDatabase.fishingMail.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos, true, player.playerStat);
    }
}

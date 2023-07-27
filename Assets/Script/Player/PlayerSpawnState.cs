using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.hurtBoxCol.enabled = false;
        player.isInvincible = true;
        player.playerSprites[1].enabled = false;
        player.weaponSprite.enabled = false;
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void ExitState(PlayerStateManager player)
    {
        player.isInvincible = false;
        player.playerSprites[1].enabled = true;
        player.weaponSprite.enabled = true;
        player.hurtBoxCol.enabled = true;
    }
}

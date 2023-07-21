using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        foreach (SpriteRenderer sr in player.playerSprites)
        {
            sr.material = player.whiteFlashMat;
        }
        player.bodyAnimator.SetTrigger("Teleport");
        player.rb.velocity = Vector2.zero;
        player.hurtBoxCol.enabled = false;
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void ExitState(PlayerStateManager player)
    {
        player.hurtBoxCol.enabled = true;
    }
}

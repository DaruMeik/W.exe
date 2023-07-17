using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float dashStartTime = 0f;
    Vector2 playerDir;
    public override void EnterState(PlayerStateManager player)
    {
        player.col.enabled = false;
        player.dashNumber++;
        player.nextDashResetTime = Time.time + 0.75f;
        playerDir = PlayerControl.Instance.pInput.Player.Move.ReadValue<Vector2>();
        if(playerDir.magnitude < 0.01f)
        {
            if (player.playerSprites[0].flipX)
            {
                playerDir = Vector2.left;
            }
            else
            {
                playerDir = Vector2.right;
            }
        }
        dashStartTime = Time.time;
        player.rb.velocity = playerDir * player.playerStat.playerSpeed * 4f;
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if(Time.time - dashStartTime >= 0.15f)
        {
            player.SwitchState(player.normalState);
        }
        if (player.dashNumber < player.playerStat.maxDash && PlayerControl.Instance.pInput.Player.Dash.WasPerformedThisFrame())
        {
            player.SwitchState(player.dashState);
        }
    }
    public override void ExitState(PlayerStateManager player)
    {
        player.col.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float dashStartTime = 0f;
    Vector2 playerDir;
    private float afterImageSpawnTime = 0f;
    public override void EnterState(PlayerStateManager player)
    {
        afterImageSpawnTime = 0f;
        player.hurtBoxCol.enabled = false;
        if (player.IFrameCoroutine != null)
            player.StopCoroutine(player.IFrameCoroutine);
        player.IFrameCoroutine = player.InvulnerableFrame();
        player.StartCoroutine(player.IFrameCoroutine);
        player.dashNumber++;
        player.nextDashResetTime = Time.time + 0.6f;
        playerDir = PlayerControl.Instance.pInput.Player.Move.ReadValue<Vector2>();
        if (playerDir.magnitude < 0.01f)
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
        if(player.playerStat.curseOfMobility == 0)
            player.rb.velocity = playerDir * player.playerStat.playerSpeed * 3f * (100f + player.speedModifier) / 100f;
        else
            player.rb.velocity = playerDir * player.playerStat.playerSpeed * 1.5f * (100f + player.speedModifier) / 100f;
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if (Time.time > afterImageSpawnTime)
        {
            afterImageSpawnTime = Time.time + 0.05f;
            GameObject temp = GameObject.Instantiate(player.afterImageVFX);
            temp.transform.position = player.transform.position;
            temp.GetComponent<SpriteRenderer>().sprite = player.playerSprites[0].sprite;
        }
        if (Time.time - dashStartTime >= 0.2f)
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
        player.hurtBoxCol.enabled = true;
        if (player.playerStat.swiftMovement)
        {
            player.GetSpeedChange(50, 1f);
        }
        if (player.playerStat.movingFort)
        {
            player.GetDefChange(50, 1f);
        }
    }
}

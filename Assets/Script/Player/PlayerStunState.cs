using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : PlayerBaseState
{
    public float stunDuration = 0f;
    private float outOfStunTime = 0f;
    private float flashTime;
    private bool show = true;
    public override void EnterState(PlayerStateManager player)
    {
        player.rb.velocity = Vector2.zero;
        foreach(SpriteRenderer sr in player.playerSprites)
            sr.color = new Color32(255, 255, 220, 255);
        flashTime = Time.time;
        show = true;
        outOfStunTime = Time.time + stunDuration;
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if (Time.time - flashTime >= 0.1f)
        {
            if (show)
            {
                foreach (SpriteRenderer sr in player.playerSprites)
                    sr.material = player.defaultMat;
            }
            else
            {
                foreach (SpriteRenderer sr in player.playerSprites)
                    sr.material = player.whiteFlashMat;
            }
            show = !show;
            flashTime = Time.time;
        }
        if (Time.time > outOfStunTime)
        {
            player.SwitchState(player.normalState);
        }
    }
    public override void ExitState(PlayerStateManager player)
    {
        foreach (SpriteRenderer sr in player.playerSprites)
            sr.color = new Color32(255, 255, 255, 255);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private float flashWhiteTimer = 0f;
    private bool show = true;
    public override void EnterState(PlayerStateManager player)
    {
        player.hurtBoxCol.enabled = false;
        player.isInvincible = true;
        player.playerSprites[1].enabled = false;
        player.weaponSprite.enabled = false;
        player.rb.velocity = Vector2.zero;
        Time.timeScale = 0.15f;
        player.bodyAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        player.bodyAnimator.SetTrigger("Dead");
        flashWhiteTimer = Time.unscaledTime;
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if (Time.unscaledTime - flashWhiteTimer >= 0.2f)
        {
            if (show)
            {
                foreach (SpriteRenderer sr in player.playerSprites)
                {
                    sr.material = player.defaultMat;
                    sr.color = new Color32(255, 255, 255, 255);
                }
            }
            else
            {
                foreach (SpriteRenderer sr in player.playerSprites)
                {
                    sr.material = player.whiteFlashMat;
                    sr.color = new Color32(255, 120, 120, 255);
                }
            }
            show = !show;
            flashWhiteTimer = Time.unscaledTime;
        }
    }
    public override void ExitState(PlayerStateManager player)
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessState : PlayerBaseState
{
    public EnemyStateManager enemy;
    private PlayerStateManager playerStateManager;
    public override void EnterState(PlayerStateManager player)
    {
        player.isInvincible = true;
        playerStateManager = player;
        player.playerSprites[1].enabled = false;
        player.weaponSprite.enabled = false;
        player.bodyAnimator.SetTrigger("Possess");
        player.rb.velocity = Vector2.zero;
        player.hurtBoxCol.enabled = false;
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit");
        player.eventBroadcast.UpdateHPNoti();
        player.bodyAnimator.SetTrigger("FinishPossess");
        player.isInvincible = false;
        player.shockWave.SetActive(true);
        player.hurtBoxCol.enabled = true;
        player.TakeDamage(-Mathf.FloorToInt(player.playerStat.maxHP * 5 / 100f));
        player.playerStat.UpdateCard(true);
    }

    public void Teleport()
    {
        Debug.Log("Teleport");
        playerStateManager.playerStat.currentWeapon[0] = enemy.enemyStat.enemyRewardWeaponID;
        playerStateManager.playerStat.currentAmmo[0] = WeaponDatabase.weaponList[playerStateManager.playerStat.currentWeapon[0]].maxAmmo;
        playerStateManager.ammo.text = ((playerStateManager.playerStat.currentAmmo[0] >= 0) ? playerStateManager.playerStat.currentAmmo[0].ToString() : "‡") + "|"
            + ((playerStateManager.playerStat.currentAmmo[1] >= 0) ? playerStateManager.playerStat.currentAmmo[1].ToString() : "‡");
        playerStateManager.UpdateWeaponSprite();
        playerStateManager.playerSprites[1].enabled = true;
        playerStateManager.weaponSprite.enabled = true;
        playerStateManager.transform.position = enemy.transform.position;
    }
}

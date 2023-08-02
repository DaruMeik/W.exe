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
        GameObject.Instantiate(player.shockWave, player.transform);
        player.TakeDamage(-Mathf.FloorToInt(player.playerStat.maxHP * (5 * (100 + player.playerStat.extraPossessHealingPerc) / 100f) / 100f));
        player.playerStat.UpdateCard(true);
    }

    public void Teleport()
    {
        Debug.Log("Teleport");
        playerStateManager.playerStat.currentWeapon[playerStateManager.playerStat.currentIndex] = enemy.enemyStat.enemyRewardWeaponID;
        playerStateManager.playerStat.currentAmmo[playerStateManager.playerStat.currentIndex]
            = Mathf.FloorToInt(WeaponDatabase.weaponList[playerStateManager.playerStat.currentWeapon[playerStateManager.playerStat.currentIndex]].maxAmmo
            * (100 + playerStateManager.playerStat.extraAmmoPerc) / 100f);
        playerStateManager.eventBroadcast.UpdateWeaponNoti();
        playerStateManager.UpdateWeaponSprite();
        playerStateManager.playerSprites[1].enabled = true;
        playerStateManager.weaponSprite.enabled = true;
        playerStateManager.transform.position = enemy.transform.position;
    }
}

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
        player.eventBroadcast.UpdateHPNoti();
        player.bodyAnimator.SetTrigger("FinishPossess");
        player.isInvincible = false;
        GameObject.Instantiate(player.shockWave, player.transform);
        player.TakeDamage(-Mathf.FloorToInt(player.playerStat.maxHP * (5 * (100 + player.playerStat.extraPossessHealingPerc) / 100f) / 100f));
        player.playerStat.UpdateCard(true);
    }

    public void Teleport()
    {
        switch (enemy.enemyStat.enemyType)
        {
            case "Miniboss":
                playerStateManager.playerStat.defaultWeapon = enemy.enemyStat.enemyRewardWeaponID;
                playerStateManager.playerStat.currentWeapon[0] = enemy.enemyStat.enemyRewardWeaponID;
                playerStateManager.playerStat.currentAmmo[0] = -1;
                break;
            default:
                playerStateManager.playerStat.currentWeapon[1] = enemy.enemyStat.enemyRewardWeaponID;
                playerStateManager.playerStat.currentAmmo[1]
                    = Mathf.CeilToInt(WeaponDatabase.weaponList[playerStateManager.playerStat.currentWeapon[1]].maxAmmo
                    * (100 + playerStateManager.playerStat.extraAmmoPerc) / 100f);
                break;
        }
        playerStateManager.eventBroadcast.UpdateWeaponNoti();
        playerStateManager.UpdateWeaponSprite();
        playerStateManager.playerSprites[1].enabled = true;
        playerStateManager.weaponSprite.enabled = true;
        playerStateManager.transform.position = enemy.transform.position;
    }
}

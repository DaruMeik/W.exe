using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessState : PlayerBaseState
{
    public EnemyStateManager enemy;
    public int index;
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
        player.playerStat.cardReadyPerc = 100;
        player.eventBroadcast.UpdateCardUINoti();
    }

    public void Teleport()
    {
        if (!playerStateManager.playerStat.wildCard)
        {
            switch (enemy.enemyStat.enemyType)
            {
                case "Miniboss":
                    playerStateManager.playerStat.defaultWeapon = enemy.enemyStat.enemyRewardWeaponID;
                    playerStateManager.playerStat.currentWeapon[index] = enemy.enemyStat.enemyRewardWeaponID;
                    playerStateManager.playerStat.currentAmmo[index] = -1;
                    break;
                default:
                    playerStateManager.playerStat.currentWeapon[index] = enemy.enemyStat.enemyRewardWeaponID;
                    float ammoModifier = 0f;
                    switch (WeaponDatabase.weaponList[playerStateManager.playerStat.currentWeapon[index]].weaponType)
                    {
                        case "Gun":
                            if (playerStateManager.playerStat.fireBullet)
                            {
                                ammoModifier += 200;
                            }
                            else if (playerStateManager.playerStat.sharpBullet)
                            {
                                ammoModifier += 200;
                            }
                            break;
                        case "Melee":
                            if (playerStateManager.playerStat.unseenBlade)
                            {
                                ammoModifier += 200;
                            }
                            if (playerStateManager.playerStat.reflectSword)
                            {
                                ammoModifier += 200;
                            }
                            break;
                        case "Charge":
                            if (playerStateManager.playerStat.fasterCharge)
                            {
                                ammoModifier += 200;
                            }
                            break;
                        case "Special":
                            if (playerStateManager.playerStat.sturdyBuild)
                            {
                                ammoModifier += 200;
                            }
                            if (playerStateManager.playerStat.goldBuild)
                            {
                                ammoModifier += 200;
                            }
                            break;
                    }
                    playerStateManager.playerStat.currentAmmo[index]
                        = Mathf.CeilToInt(WeaponDatabase.weaponList[playerStateManager.playerStat.currentWeapon[index]].maxAmmo
                        * (100 + playerStateManager.playerStat.extraAmmoPerc + ammoModifier) / 100f);
                    break;
            }
            playerStateManager.eventBroadcast.UpdateWeaponNoti();
            playerStateManager.UpdateWeaponSprite();
        }

        playerStateManager.playerSprites[1].enabled = true;
        playerStateManager.weaponSprite.enabled = true;
        playerStateManager.transform.position = enemy.transform.position;
    }
}

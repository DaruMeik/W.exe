using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessState : PlayerBaseState
{
    public EnemyStateManager enemy;
    private PlayerStateManager playerStateManager;
    public override void EnterState(PlayerStateManager player)
    {
        playerStateManager = player;
        player.playerSprites[1].enabled = false;
        player.weaponSprite.enabled = false;
        player.bodyAnimator.SetTrigger("Possess");
        player.rb.velocity = Vector2.zero;
        player.col.enabled = false;
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit");
        player.bodyAnimator.SetTrigger("FinishPossess");
        player.shockWave.SetActive(true);
        player.col.enabled = true;
        player.playerStat.UpdateCard(true);
    }

    public void Teleport()
    {
        Debug.Log("Teleport");
        playerStateManager.playerStat.currentWeapon[0] = enemy.enemyStat.enemyRewardWeaponID;
        playerStateManager.playerStat.currentAmmo[0] = WeaponDatabase.weaponList[playerStateManager.playerStat.currentWeapon[0]].maxAmmo;
        playerStateManager.UpdateWeaponSprite();
        playerStateManager.playerSprites[1].enabled = true;
        playerStateManager.weaponSprite.enabled = true;
        playerStateManager.transform.position = enemy.transform.position;
    }
}

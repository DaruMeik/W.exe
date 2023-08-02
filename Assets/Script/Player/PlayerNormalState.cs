using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerBaseState
{
    private Vector2 playerDir;
    private Vector2 mousePos;
    private float lookAngle;

    public bool isShooting;
    private IEnumerator shootingCoroutine;

    private float nextTimeToSwitch = 0f;
    public override void EnterState(PlayerStateManager player)
    {
    }
    public override void UpdateState(PlayerStateManager player)
    {
        #region map
        if (PlayerControl.Instance.pInput.Player.Map.WasPerformedThisFrame())
        {
            player.map.SetActive(!player.map.activeSelf);
        }
        #endregion

        if (player.isInUI)
            return;
        #region aiming
        mousePos = Camera.main.ScreenToWorldPoint(PlayerControl.Instance.pInput.Player.Look.ReadValue<Vector2>());
        lookAngle = Mathf.Atan2((mousePos - (Vector2)player.weaponPivotPoint.position).y, (mousePos - (Vector2)player.weaponPivotPoint.position).x) * Mathf.Rad2Deg;
        FlipSprite(player);
        #endregion

        #region movement
        playerDir = PlayerControl.Instance.pInput.Player.Move.ReadValue<Vector2>();
        player.rb.velocity = playerDir * (player.playerStat.playerSpeed * (100 - player.speedPenalty) / 100f);
        player.bodyAnimator.SetFloat("Velocity", player.rb.velocity.magnitude);
        if ((player.rb.velocity.x > 0 && player.playerSprites[0].flipX) || (player.rb.velocity.x < 0 && !player.playerSprites[0].flipX))
            player.bodyAnimator.SetBool("isReversed", true);
        else
            player.bodyAnimator.SetBool("isReversed", false);
        #endregion

        #region switch weapon
        float scrollVal = PlayerControl.Instance.pInput.Player.Scroll.ReadValue<Vector2>().y;
        if (Time.time > nextTimeToSwitch)
        {
            if (PlayerControl.Instance.pInput.Player.Switch.WasPerformedThisFrame())
            {
                isShooting = false;
                int temp;
                temp = player.playerStat.currentWeapon[0];
                player.playerStat.currentWeapon[0] = player.playerStat.currentWeapon[1];
                player.playerStat.currentWeapon[1] = temp;

                temp = player.playerStat.currentAmmo[0];
                player.playerStat.currentAmmo[0] = player.playerStat.currentAmmo[1];
                player.playerStat.currentAmmo[1] = temp;
                player.UpdateWeaponSprite();

                nextTimeToSwitch = Time.time + 0.5f;
            }
            else if (PlayerControl.Instance.pInput.Player.FirstWeapon.WasPerformedThisFrame() || (scrollVal > 0.01 && player.playerStat.currentIndex != 0))
            {
                isShooting = false;
                player.playerStat.currentIndex = 0;
                player.UpdateWeaponSprite();
                nextTimeToSwitch = Time.time + 0.5f;
            }
            else if (PlayerControl.Instance.pInput.Player.SecondWeapon.WasPerformedThisFrame() || (scrollVal < -0.01 && player.playerStat.currentIndex != 1))
            {
                isShooting = false;
                player.playerStat.currentIndex = 1;
                player.UpdateWeaponSprite();
                nextTimeToSwitch = Time.time + 0.5f;
            }
        }
        #endregion

        #region shooting
        if (PlayerControl.Instance.pInput.Player.Shoot.IsPressed())
        {
            if (shootingCoroutine == null)
            {
                isShooting = true;
                shootingCoroutine = Shooting(player);
                player.StartCoroutine(shootingCoroutine);
            }
        }
        if (PlayerControl.Instance.pInput.Player.Shoot.WasReleasedThisFrame())
        {
            isShooting = false;
        }
        #endregion

        #region sending calling card
        if (PlayerControl.Instance.pInput.Player.Throw.WasPerformedThisFrame())
        {
            if (player.enemy != null)
            {
                if (!player.enemy.isExploding)
                {
                    player.enemy.isPossessed = true;
                    player.enemy.animator.SetBool("Marked", true);
                    player.enemy.animator.SetTrigger("Explode");
                    player.possessState.enemy = player.enemy;
                    player.SwitchState(player.possessState);
                }
            }
            else if (player.playerStat.hasCard)
            {
                player.playerStat.UpdateCard(false);
                player.handAnimator.SetTrigger("Throw");
                player.offHand.player = player;
                player.offHand.mousePos = mousePos;
            }
        }
        #endregion

        #region dashing
        if (player.dashNumber < player.playerStat.maxDash && PlayerControl.Instance.pInput.Player.Dash.WasPerformedThisFrame())
        {
            player.SwitchState(player.dashState);
        }
        #endregion

        #region interact
        if (PlayerControl.Instance.pInput.Player.Interact.WasPerformedThisFrame())
        {
            if (player.interactableObj.Count > 0)
            {
                player.interactingObj = player.interactableObj[player.interactableObj.Count - 1];
                switch (player.interactingObj.tag)
                {
                    case "Portal":
                        player.SwitchState(player.teleportState);
                        break;
                    case "NPC":
                        player.interactingObj.GetComponent<NPC>().TurnOnText();
                        break;
                    case "OneTime":
                        player.interactingObj.GetComponent<Onetime>().Interact();
                        break;
                }
                isShooting = false;
            }
        }
        #endregion
    }
    public override void ExitState(PlayerStateManager player)
    {
        if (shootingCoroutine != null)
            isShooting = false;
    }
    private void FlipSprite(PlayerStateManager player)
    {
        if ((lookAngle > -89f) && (lookAngle < 89f))
        {
            player.weaponPivotPoint.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            player.weaponPivotPoint.localScale = new Vector3(1f, 1f, 1f);
            foreach (SpriteRenderer sr in player.playerSprites)
            {
                sr.flipX = false;
            }
        }
        else if ((lookAngle < -101f) || (lookAngle > 101f))
        {
            player.weaponPivotPoint.rotation = Quaternion.Euler(0f, 0f, 180f + lookAngle);
            player.weaponPivotPoint.localScale = new Vector3(-1f, 1f, 1f);
            foreach (SpriteRenderer sr in player.playerSprites)
            {
                sr.flipX = true;
            }
        }
    }
    private IEnumerator Shooting(PlayerStateManager player)
    {
        Weapon weapon = WeaponDatabase.weaponList[player.playerStat.currentWeapon[player.playerStat.currentIndex]];
        weapon.weaponBaseEffect.weaponPoint = player.weaponPivotPoint;
        player.speedPenalty += weapon.speedPenalty;
        while (isShooting)
        {
            if (weapon.triggerId != null)
            {
                player.trigger.weapon = weapon;
                player.trigger.aimPos = mousePos;
                player.weaponAnimator.SetTrigger(weapon.triggerId);
            }
            else if(weapon.weaponType == "Charge")
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, ref player.spawnedBullet);
            }
            else
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, ref player.spawnedBullet);
                player.playerStat.currentAmmo[player.playerStat.currentIndex]--;
                player.eventBroadcast.UpdateWeaponNoti();
                if (player.playerStat.currentAmmo[player.playerStat.currentIndex] == 0)
                {
                    player.playerStat.currentWeapon[player.playerStat.currentIndex] = 0;
                    player.playerStat.currentAmmo[player.playerStat.currentIndex] = -1;
                    player.eventBroadcast.UpdateWeaponNoti();
                    player.UpdateWeaponSprite();
                    isShooting = false;
                }
            }
            yield return new WaitForSeconds(5f / weapon.atkSpd);
        }
        if (weapon.weaponType == "Charge")
        {
            weapon.weaponBaseEffect.Release(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, ref player.spawnedBullet);
            player.playerStat.currentAmmo[player.playerStat.currentIndex]--;
            player.eventBroadcast.UpdateWeaponNoti();
            if (player.playerStat.currentAmmo[player.playerStat.currentIndex] == 0)
            {
                player.playerStat.currentWeapon[player.playerStat.currentIndex] = 0;
                player.playerStat.currentAmmo[player.playerStat.currentIndex] = -1;
                player.eventBroadcast.UpdateWeaponNoti();
                player.UpdateWeaponSprite();
                isShooting = false;
            }
        }
        player.speedPenalty -= weapon.speedPenalty;
        if (shootingCoroutine != null)
            player.StopCoroutine(shootingCoroutine);
        shootingCoroutine = null;

    }
}

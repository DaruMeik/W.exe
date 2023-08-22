using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerBaseState
{
    private Vector2 playerDir;
    private Vector2 mousePos;
    private float lookAngle;

    public bool isShooting1;
    public bool isShooting2;
    private IEnumerator shootingCoroutine1;
    private IEnumerator shootingCoroutine2;
    private float rechargeTime = 0f;

    public float nextTimeToShoot1 = 0f;
    public float nextTimeToShoot2 = 0f;
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
        player.rb.velocity = playerDir * (player.playerStat.playerSpeed * (100 - player.speedPenalty + player.speedModifier) / 100f);
        player.bodyAnimator.SetFloat("Velocity", player.rb.velocity.magnitude);
        if ((player.rb.velocity.x > 0 && player.playerSprites[0].flipX) || (player.rb.velocity.x < 0 && !player.playerSprites[0].flipX))
            player.bodyAnimator.SetBool("isReversed", true);
        else
            player.bodyAnimator.SetBool("isReversed", false);
        #endregion

        #region shooting
        if (PlayerControl.Instance.pInput.Player.Shoot.IsPressed())
        {
            if (shootingCoroutine1 == null && Time.time > nextTimeToShoot1)
            {
                isShooting2 = false;
                player.playerStat.currentIndex = 0;
                player.UpdateWeaponSprite();
                isShooting1 = true;
                shootingCoroutine1 = Shooting1(player);
                player.StartCoroutine(shootingCoroutine1);
            }
        }
        if (PlayerControl.Instance.pInput.Player.SubWeapon.IsPressed())
        {
            if (shootingCoroutine2 == null && Time.time > nextTimeToShoot2)
            {
                isShooting1 = false;
                player.playerStat.currentIndex = 1;
                player.UpdateWeaponSprite();
                isShooting2 = true;
                shootingCoroutine2 = Shooting2(player);
                player.StartCoroutine(shootingCoroutine2);
            }
        }
        if (PlayerControl.Instance.pInput.Player.Shoot.WasReleasedThisFrame())
        {
            isShooting1 = false;
        }
        if (PlayerControl.Instance.pInput.Player.SubWeapon.WasReleasedThisFrame())
        {
            isShooting2 = false;
        }
        #endregion

        #region sending calling card
        if (PlayerControl.Instance.pInput.Player.Throw.WasPerformedThisFrame())
        {
            if (player.playerStat.cardReadyPerc == 100)
            {
                player.eventBroadcast.SendingCardNoti();
                player.playerStat.cardReadyPerc = 0;
                rechargeTime = Time.time + 10f;
                player.handAnimator.SetTrigger("Throw");
                player.offHand.player = player;
                player.offHand.mousePos = mousePos;
            }
        }
        if(player.playerStat.cardReadyPerc != 100)
        {
            if (Time.time < rechargeTime)
            {
                Debug.Log("Here");
                player.playerStat.cardReadyPerc = Mathf.Min(100, Mathf.Max(0, Mathf.FloorToInt((10f - rechargeTime + Time.time) * 100 / 10f)));
                Debug.Log(player.playerStat.cardReadyPerc);
                player.eventBroadcast.UpdateCardUINoti();
            }
            else if (Time.time >= rechargeTime)
            {
                player.playerStat.cardReadyPerc = 100;
                player.eventBroadcast.UpdateCardUINoti();
            }
        }
        if(PlayerControl.Instance.pInput.Player.Pos1.IsPressed() || PlayerControl.Instance.pInput.Player.Pos2.IsPressed())
        {
            if (player.enemy != null)
            {
                if (!player.enemy.isExploding)
                {
                    player.enemy.isPossessed = true;
                    player.enemy.animator.SetBool("Marked", true);
                    player.enemy.animator.SetTrigger("Explode");
                    if (PlayerControl.Instance.pInput.Player.Pos1.IsPressed())
                    {
                        player.possessState.index = 0;
                    }
                    if (PlayerControl.Instance.pInput.Player.Pos2.IsPressed())
                    {
                        player.possessState.index = 1;
                    }
                    player.possessState.enemy = player.enemy;
                    player.SwitchState(player.possessState);
                }
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
                isShooting1 = false;
            }
        }
        #endregion
    }
    public override void ExitState(PlayerStateManager player)
    {
        if (shootingCoroutine1 != null)
            isShooting1 = false;
        if (shootingCoroutine2 != null)
            isShooting2 = false;
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
    private IEnumerator Shooting1(PlayerStateManager player)
    {
        Weapon weapon = WeaponDatabase.weaponList[player.playerStat.currentWeapon[0]];
        weapon.weaponBaseEffect.weaponPoint = player.weaponPivotPoint;
        player.speedPenalty += weapon.speedPenalty;
        while (isShooting1)
        {
            if (weapon.triggerId != null)
            {
                player.trigger.weapon = weapon;
                player.trigger.aimPos = mousePos;
                player.weaponAnimator.SetTrigger(weapon.triggerId);
            }
            else if (weapon.weaponType == "Charge")
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, player.rb, ref player.spawnedBullet);
            }
            else
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, player.rb, ref player.spawnedBullet);
                if (player.playerStat.currentAmmo[0] > 0)
                    player.playerStat.currentAmmo[0]--;
                player.eventBroadcast.UpdateWeaponNoti();
                if (player.playerStat.currentAmmo[0] == 0)
                {
                    player.normalState.nextTimeToShoot1 = Time.time + 1f;
                    isShooting1 = false;
                    player.playerStat.currentWeapon[0] = player.playerStat.defaultWeapon;
                    player.playerStat.currentAmmo[0] = -1;
                    player.eventBroadcast.UpdateWeaponNoti();
                    player.UpdateWeaponSprite();
                }
            }
            yield return new WaitForSeconds(5f / weapon.atkSpd);
        }
        if (weapon.weaponType == "Charge")
        {
            weapon.weaponBaseEffect.Release(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, ref player.spawnedBullet);
            if (player.playerStat.currentAmmo[0] > 0) 
                player.playerStat.currentAmmo[0]--;
            player.eventBroadcast.UpdateWeaponNoti();
            if (player.playerStat.currentAmmo[0] == 0)
            {
                player.normalState.nextTimeToShoot1 = Time.time + 1f;
                isShooting1 = false;
                player.playerStat.currentWeapon[0] = player.playerStat.defaultWeapon;
                player.playerStat.currentAmmo[0] = -1;
                player.eventBroadcast.UpdateWeaponNoti();
                player.UpdateWeaponSprite();
            }
        }
        player.speedPenalty -= weapon.speedPenalty;
        if (shootingCoroutine1 != null)
            player.StopCoroutine(shootingCoroutine1);
        shootingCoroutine1 = null;
    }
    private IEnumerator Shooting2(PlayerStateManager player)
    {
        Weapon weapon = WeaponDatabase.weaponList[player.playerStat.currentWeapon[1]];
        weapon.weaponBaseEffect.weaponPoint = player.weaponPivotPoint;
        player.speedPenalty += weapon.speedPenalty;
        while (isShooting2)
        {
            if (weapon.triggerId != null)
            {
                player.trigger.weapon = weapon;
                player.trigger.aimPos = mousePos;
                player.weaponAnimator.SetTrigger(weapon.triggerId);
            }
            else if (weapon.weaponType == "Charge")
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, player.rb, ref player.spawnedBullet);
            }
            else
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, player.rb, ref player.spawnedBullet);
                if (player.playerStat.currentAmmo[1] > 0)
                    player.playerStat.currentAmmo[1]--;
                player.eventBroadcast.UpdateWeaponNoti();
                if (player.playerStat.currentAmmo[1] == 0)
                {
                    player.normalState.nextTimeToShoot2 = Time.time + 1f;
                    isShooting2 = false;
                    player.playerStat.currentWeapon[1] = player.playerStat.defaultWeapon;
                    player.playerStat.currentAmmo[1] = -1;
                    player.eventBroadcast.UpdateWeaponNoti();
                    player.UpdateWeaponSprite();
                }
            }
            yield return new WaitForSeconds(5f / weapon.atkSpd);
        }
        if (weapon.weaponType == "Charge")
        {
            weapon.weaponBaseEffect.Release(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat, ref player.spawnedBullet);
            if (player.playerStat.currentAmmo[1] > 0)
                player.playerStat.currentAmmo[1]--;
            player.eventBroadcast.UpdateWeaponNoti();
            if (player.playerStat.currentAmmo[1] == 0)
            {
                player.normalState.nextTimeToShoot2 = Time.time + 1f;
                isShooting2 = false;
                player.playerStat.currentWeapon[1] = player.playerStat.defaultWeapon;
                player.playerStat.currentAmmo[1] = -1;
                player.eventBroadcast.UpdateWeaponNoti();
                player.UpdateWeaponSprite();
            }
        }
        player.speedPenalty -= weapon.speedPenalty;
        if (shootingCoroutine2 != null)
            player.StopCoroutine(shootingCoroutine2);
        shootingCoroutine2 = null;
    }
}

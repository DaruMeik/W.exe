using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerBaseState
{
    private Vector2 playerDir;
    private Vector2 mousePos;
    private float lookAngle;

    private bool isShooting;
    private IEnumerator shootingCoroutine;

    private GameObject mouseOverObj;
    public override void EnterState(PlayerStateManager player)
    {
    }
    public override void UpdateState(PlayerStateManager player)
    {

        #region aiming
        mousePos = Camera.main.ScreenToWorldPoint(PlayerControl.Instance.pInput.Player.Look.ReadValue<Vector2>());
        lookAngle = Mathf.Atan2((mousePos - (Vector2)player.weaponPivotPoint.position).y, (mousePos - (Vector2)player.weaponPivotPoint.position).x) * Mathf.Rad2Deg;
        FlipSprite(player);
        #endregion

        #region check if can possess
        Ray ray = Camera.main.ScreenPointToRay(PlayerControl.Instance.pInput.Player.Look.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Possession"))
            mouseOverObj = hit.collider.gameObject;
        else
            mouseOverObj = null;
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
            if (mouseOverObj != null)
            {
                EnemyStateManager enemy = mouseOverObj.GetComponentInParent<EnemyStateManager>();
                if (!enemy.isExploding)
                {
                    enemy.isPossessed = true;
                    enemy.animator.SetBool("Marked", true);
                    enemy.animator.SetTrigger("Explode");
                    player.possessState.enemy = enemy;
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
        Weapon weapon = WeaponDatabase.weaponList[player.playerStat.currentWeapon[0]];
        player.speedPenalty += weapon.speedPenalty;
        while (isShooting)
        {
            if (weapon.triggerId != null)
            {
                player.trigger.weapon = weapon;
                player.trigger.aimPos = mousePos;
                player.weaponAnimator.SetTrigger(weapon.triggerId);
            }
            else
            {
                weapon.weaponBaseEffect.ApplyEffect(player.weaponPivotPoint.position, mousePos + Random.insideUnitCircle * 1.5f * (100 - weapon.accuracy) / 100f, true, player.playerStat);
                player.playerStat.currentAmmo[0]--;
            }
            if (player.playerStat.currentAmmo[0] == 0)
            {
                player.playerStat.currentWeapon[0] = 0;
                player.UpdateWeaponSprite();
                isShooting = false;
            }
            yield return new WaitForSeconds(5f / weapon.atkSpd);
        }

        player.speedPenalty -= weapon.speedPenalty;
        if (shootingCoroutine != null)
            player.StopCoroutine(shootingCoroutine);
        shootingCoroutine = null;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea11_ChargeSpear : Bullet
{
    public Vector3 endPoint;
    public Rigidbody2D userRB;
    public PlayerStateManager playerState;
    public EnemyStateManager enemyState;

    protected override void Start()
    {
        base.Start();
        if (bySelf)
        {
            if (playerState.beingControlledBy != null)
            {
                playerState.transform.parent = null;
                Destroy(playerState.beingControlledBy);
            }
            playerState.beingControlledBy = gameObject;
            playerState.transform.parent = gameObject.transform;
            playerState.normalState.isShooting1 = false;
            playerState.normalState.isShooting2 = false;
        }
        else
        {
            if (enemyState.beingControlledBy != null)
            {
                enemyState.transform.parent = null;
                Destroy(enemyState.beingControlledBy);
            }
            enemyState.beingControlledBy = gameObject;
            enemyState.transform.parent = gameObject.transform;
        }
        rb.velocity = (endPoint - (Vector3)spawnPos).normalized * 12f;
    }
    protected override void Update()
    {
        if (!ready)
            return;
        if(Vector2.Distance(gameObject.transform.position, endPoint) < 1f)
        {
            try
            {
                if (bySelf)
                {
                    playerState.rb.isKinematic = false;
                    playerState.transform.parent = null;
                    playerState.beingControlledBy = null;
                    playerState.GetStun(0.5f);
                    Destroy(gameObject);
                }
                else
                {
                    enemyState.rb.isKinematic = false;
                    enemyState.transform.parent = null;
                    enemyState.beingControlledBy = null;
                    enemyState.animator.SetTrigger("Stop");
                    enemyState.GetStun(1.5f, false);
                    Destroy(gameObject);
                }
            }
            catch { Destroy(gameObject); };
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready)
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                float attackModifier = 0f;
                if (ID == playerStat.currentWeapon[0])
                    attackModifier += playerStat.defaultWeaponAtkUpPerc;
                if (isBurning)
                    temp.GetBurn(1);
                if (playerStat.unseenBlade &&
                    Vector3.Dot((temp.transform.position - (Vector3)spawnPos).normalized, new Vector3(temp.enemySprite.transform.localScale.x, 0f, 0f)) > 0)
                {
                    attackModifier += 50;
                }
                if (isCrit)
                {
                    attackModifier += 200;
                    Instantiate(critVFX).transform.position = collision.transform.position;
                }
                temp.GetStun(0.1f, false);
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet")
            {
                Bullet temp = collision.GetComponentInParent<Bullet>();
                if (temp != null)
                {
                    if (temp.HP > 0)
                    {
                        float attackModifier = 0f;
                        if (ID == playerStat.currentWeapon[0])
                            attackModifier += playerStat.defaultWeaponAtkUpPerc;
                        if (isCrit)
                        {
                            attackModifier += 200;
                            Instantiate(critVFX).transform.position = collision.transform.position;
                        }
                        temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
                    }
                    else if (temp.isDestroyable)
                    {
                        if (playerStat.reflectSword)
                        {
                            temp.rb.velocity = -temp.rb.velocity;
                            temp.bySelf = true;
                            collision.tag = "PlayerBullet";
                            temp.firstHit = true;
                        }
                        else
                        {
                            Destroy(collision.gameObject);
                        }
                    }
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "PlayerBullet")
            {
                Bullet temp = collision.GetComponentInParent<Bullet>();
                if (temp != null)
                {
                    if (temp.HP > 0)
                    {
                        temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
                    }
                    else if (temp.isDestroyable)
                    {
                        Destroy(collision.gameObject);
                    }
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
        }
    }
}

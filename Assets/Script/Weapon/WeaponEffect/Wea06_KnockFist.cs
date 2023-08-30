using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea06_KnockFist : Bullet
{
    public Vector2 dir;
    protected override void OnEnable()
    {
        base.OnEnable();
        spawnTime = Time.time;
    }
    protected override void Update()
    {
        if (!ready)
            return;
        if (Time.time - spawnTime > 0.1f)
        {
            Destroy(gameObject);
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
                temp.GetStun(0.25f, false);
                if (temp.beingControlledBy != null)
                {
                    temp.rb.isKinematic = false;
                    temp.transform.parent = null;
                    Destroy(temp.beingControlledBy);
                    temp.beingControlledBy = null;
                    temp.animator.SetTrigger("Stop");
                    temp.GetStun(2f, false);
                }
                temp.rb.AddForce((temp.transform.position - gameObject.transform.position).normalized * 10f, ForceMode2D.Impulse);
                temp.rb.AddForce(dir * 9f, ForceMode2D.Impulse);
                if (isBurning)
                    temp.GetBurn(1);
                if (ID == playerStat.currentWeapon[0])
                    attackModifier += playerStat.defaultWeaponAtkUpPerc;
                if (playerStat.unseenBlade &&
                    Vector3.Dot((temp.transform.position - (Vector3)spawnPos).normalized, new Vector3(temp.enemySprite.transform.localScale.x, 0f, 0f)) > 0)
                {
                    attackModifier += 50;
                }
                if (playerStat.critableGun && Random.Range(0, 100) > 90)
                {
                    attackModifier += 200;
                    Instantiate(critVFX).transform.position = collision.transform.position;
                }
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                Bullet temp = collision.GetComponentInChildren<Bullet>();
                if (temp != null)
                {
                    if (temp.isPushable)
                    {
                        collision.tag = "PlayerBullet";
                        temp.bySelf = bySelf;
                        temp.rb.velocity = dir * Mathf.Max(9f, temp.rb.velocity.magnitude);
                    }
                    else if (collision.tag == "EnemyBullet")
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
                        if (temp.isDestroyable)
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
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                {
                    temp.Throw();
                    temp.rb.AddForce(dir * 12f, ForceMode2D.Impulse);
                }
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.GetStun(0.25f);
                if (temp.beingControlledBy != null)
                {
                    temp.rb.isKinematic = false;
                    temp.transform.parent = null;
                    Destroy(temp.beingControlledBy);
                    temp.beingControlledBy = null;
                }
                temp.rb.AddForce(dir * 9f, ForceMode2D.Impulse);
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                Bullet temp = collision.GetComponentInChildren<Bullet>();
                if (temp != null && temp.isPushable)
                {
                    collision.tag = "EnemyBullet";
                    temp.bySelf = bySelf;
                    temp.rb.velocity = dir * Mathf.Max(9f, temp.rb.velocity.magnitude);
                }
                else
                if (collision.tag == "PlayerBullet")
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
                        Destroy(collision.gameObject);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                {
                    temp.Throw();
                    temp.rb.AddForce(dir * 12f, ForceMode2D.Impulse);
                }
            }
        }
    }
}

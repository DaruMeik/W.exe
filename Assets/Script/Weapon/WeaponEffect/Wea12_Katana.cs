using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea12_Katana : Bullet
{
    public float chargeAmount;
    public PlayerStateManager playerState;
    public EnemyStateManager enemyState;
    public GameObject hitBox;
    public Vector2 endPoint;
    protected override void Update()
    {
        if (!ready)
            return;
        if (Time.time - spawnTime > 0.2f && hitBox.activeSelf)
        {
            Destroy(gameObject);
        }

    }
    public void TurnOnHitBox()
    {
        spawnTime = Time.time;
        hitBox.SetActive(true);
    }
    public void Teleport()
    {
        Vector2 telePoint = spawnPos + (endPoint - spawnPos).normalized * (100 + 3 * chargeAmount) / 100f;
        RaycastHit2D result = Physics2D.CapsuleCast(spawnPos, new Vector2(0.65f, 1.25f), CapsuleDirection2D.Vertical, 0f, (endPoint - spawnPos).normalized, (100 + 3 * chargeAmount) / 100f, LayerMask.GetMask("Wall"));
        if (result)
        {
            telePoint = result.centroid;
        }
        if (bySelf)
        {
            atkPerc = playerStat.atkPerc;
            if (playerStat.BEEGBomb)
                gameObject.transform.localScale *= 1.5f;
            transform.localScale = new Vector3((telePoint - spawnPos).magnitude, 1f, 1f);
            playerState.rb.MovePosition((Vector2)telePoint - Vector2.up * playerState.GetComponent<CapsuleCollider2D>().size.y/2f);
        }
        else
        {
            atkPerc = 0;
            transform.localScale = new Vector3((telePoint - spawnPos).magnitude, 1f, 1f);
            enemyState.rb.MovePosition((Vector2)telePoint - Vector2.up * enemyState.GetComponent<CapsuleCollider2D>().size.y / 2f);
        }
        TurnOnHitBox();
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
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
                temp.GetStun(0.25f, false);
                temp.rb.AddForce(dir * 9f, ForceMode2D.Impulse);
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
                if (isBurning)
                    temp.GetBurn(6f);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet")
            {
                Destroy(collision.gameObject);
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
                temp.rb.AddForce(dir * 9f, ForceMode2D.Impulse);
                temp.TakeDamage(WeaponDatabase.weaponList[ID].power);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "PlayerBullet")
            {
                Destroy(collision.gameObject);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea02_ShortSword : Bullet
{
    protected override void Update()
    {
        if (!ready)
            return;
        if(Time.time - spawnTime > 0.1f)
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
                if (isBurning)
                    temp.GetBurn(2.5f);
                if(playerStat.unseenBlade && 
                    Vector3.Dot((temp.transform.position - (Vector3)spawnPos).normalized, new Vector3(temp.enemySprite.transform.localScale.x,0f,0f)) > 0)
                {
                    attackModifier += 50;
                }
                if (playerStat.critable && Random.Range(0, 100) >= 90)
                    attackModifier += 200;
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet" && collision.GetComponent<Bullet>().isDestroyable)
            {
                Bullet temp = collision.GetComponent<Bullet>();
                if (temp != null && playerStat.goodReflex)
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
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0,Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
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
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0,Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
        }
    }
}

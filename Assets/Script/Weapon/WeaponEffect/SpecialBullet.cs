using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        atkPerc = 1000;
    }
    protected override void Update()
    {
        if (!ready)
            return;
        if (Vector2.Distance(spawnPos, transform.position) > 30f)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                float attackModifier = 0f;
                if (isCrit)
                {
                    attackModifier += 200;
                    Instantiate(critVFX).transform.position = collision.transform.position;
                }
                if (isBurning)
                    temp.GetBurn(1);
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[0].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[0].power * (100 + atkPerc) / 100f)));
            }
            if (playerStat.sharpBullet && Random.Range(0, 100) >= 50)
            {
                firstHit = true;
                return;
            }
            Destroy(gameObject);
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[0].power * (100 + atkPerc) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[0].power * (100 + atkPerc) / 100f)));
            }
            Destroy(gameObject);
        }
    }
}
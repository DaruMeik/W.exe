using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea05_ZapCanon : Bullet
{
    public int chargeAmount = 0;
    protected override void Update()
    {
        if (!ready)
            return;
        if (Vector2.Distance(spawnPos, transform.position) > 20f)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.tag == "Low")
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (chargeAmount < 100)
                firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                collision.gameObject.GetComponent<EnemyStateManager>().GetStun(0.5f * (100 + 2 * chargeAmount) / 100f, false);
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f * (100 + 3 * chargeAmount) / 100f));
                if (isBurning)
                    temp.GetBurn(6f);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || chargeAmount < 100)
            {
                Destroy(gameObject);
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                collision.gameObject.GetComponent<PlayerStateManager>().GetStun(0.5f * (100 + 1.5f * chargeAmount) / 100f);
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f * (100 + 2 * chargeAmount) / 100f));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            Destroy(gameObject);
        }
    }
}
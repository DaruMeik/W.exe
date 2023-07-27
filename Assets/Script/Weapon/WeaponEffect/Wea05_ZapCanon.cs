using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea05_ZapCanon : MonoBehaviour
{
    public int ID = 5;
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public bool ready = false;
    private bool firstHit = true;
    public int atkPerc;
    public int chargeAmount = 0;
    private void OnEnable()
    {
        ready = false;
        firstHit = true;
    }
    private void Update()
    {
        if (!ready)
            return;
        if (Vector2.Distance(spawnPos, transform.position) > 10f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (chargeAmount < 100)
                firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                collision.gameObject.GetComponent<EnemyStateManager>().GetStun(0.5f * (100 + 2 * chargeAmount) / 100f);
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f * (100 + 2 * chargeAmount) / 100f));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || chargeAmount < 100)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea00_Buster : MonoBehaviour
{
    public int ID = 0;
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public bool ready = false;
    public int atkPerc;
    private bool firstHit = true;
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
        if (!ready || !firstHit ||collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            Destroy(gameObject);
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(WeaponDatabase.weaponList[ID].power);
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

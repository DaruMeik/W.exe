using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea02_ShortSword : MonoBehaviour
{
    public int ID = 2;
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public float spawnTime;
    public bool ready = false;
    private void OnEnable()
    {
        spawnTime = Time.time;
        ready = false;
    }
    private void Update()
    {
        if (!ready)
            return;
        if(Time.time - spawnTime > 0.1f)
        {
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready)
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                temp.TakeDamage(WeaponDatabase.weaponList[ID].power);
            }
            if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet")
            {
                Destroy(collision.gameObject);
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(WeaponDatabase.weaponList[ID].power);
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "PlayerBullet")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}

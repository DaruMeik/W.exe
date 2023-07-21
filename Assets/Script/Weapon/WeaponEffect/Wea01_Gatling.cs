using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea01_Gatling : MonoBehaviour
{
    public int ID = 1;
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public bool ready = false;
    private bool firstHit = true;
    public int atkPerc;
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
            Destroy(gameObject);
        }
    }
}
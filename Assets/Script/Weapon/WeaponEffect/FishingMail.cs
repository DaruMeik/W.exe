using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMail : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public bool isPickupable = false;
    public bool ready = false;
    public PlayerStat playerStat;
    private void OnEnable()
    {
        isPickupable = false;
        ready = false;
    }
    private void Update()
    {
        if(!bySelf && Vector2.Distance(spawnPos, rb.position) < 0.75f)
        {
            transform.Translate(Vector2.down * Time.deltaTime);
        }
        if (!ready)
            return;
        if (Vector2.Distance(spawnPos, transform.position) > 15f)
        {
            isPickupable = true;
            rb.velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready)
            return;
        if (!isPickupable && collision.tag != "Player")
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                isPickupable = true;
                rb.velocity = Vector2.zero;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                temp.marked = true;
                temp.mark.SetActive(true);
                temp.TakeDamage(WeaponDatabase.fishingMail.power);
                Destroy(gameObject);
            }
        }
        else if (isPickupable && collision.tag == "Player")
        {
            playerStat.UpdateCard(true);
            Destroy(gameObject);
        }
    }
}

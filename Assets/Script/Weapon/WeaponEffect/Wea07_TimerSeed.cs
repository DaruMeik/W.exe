using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea07_TimerSeed : Bullet
{
    public Collider2D col;
    public Animator animator;
    private bool isBloom = false;
    protected  override void Update()
    {
        if (rb.velocity.magnitude > 0.25f && Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Wall")))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            animator.SetTrigger("Trigger");
        }
        base.Update();
    }
    public void Bloom()
    {
        isBloom = true;
        col.enabled = false;
        col.enabled = true;
    }
    public void Explode()
    {
        eventBroadcast.CameraShakeNoti(0.5f, 2f);
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Explode");
        col.enabled = false;
        Collider2D[] inZoneObj = null;
        if (bySelf)
        {
            inZoneObj = Physics2D.OverlapCircleAll(gameObject.transform.position + Vector3.up * 0.3f, 2.5f, LayerMask.GetMask("EnemyHurtBox", "Obstacle"));
        }
        else
        {
            inZoneObj = Physics2D.OverlapCircleAll(gameObject.transform.position + Vector3.up * 0.3f, 2.5f, LayerMask.GetMask("PlayerHurtBox", "Obstacle"));
        }
        foreach (Collider2D obj in inZoneObj)
        {
            if (obj.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager enemy = obj.GetComponent<EnemyStateManager>();
                if (enemy.currentState != enemy.deadState)
                {
                    enemy.GetStun(1f, false);
                    enemy.GetBurn(6f);
                    enemy.rb.AddForce((enemy.transform.position - gameObject.transform.position).normalized * 10f, ForceMode2D.Impulse);
                    float attackModifier = 0;
                    if (playerStat.critable && Random.Range(0, 100) > 90)
                        attackModifier += 200;
                    enemy.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
                }
            }
            else if (obj.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager player = obj.GetComponent<PlayerStateManager>();
                player.GetStun(1f);
                player.GetBurn(6f);
                player.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
                player.rb.AddForce((player.transform.position - gameObject.transform.position).normalized * 10f, ForceMode2D.Impulse);
            }
            else if (obj.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                obj.GetComponent<DestroyableObstacle>().TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
        }
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || !firstHit || !isBloom || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.tag == "Low")
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            firstHit = false;
            animator.SetTrigger("Trigger");
            if(rb.velocity.magnitude > 0.01f)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                transform.parent = collision.transform;
                transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            animator.SetTrigger("Trigger");
            if (rb.velocity.magnitude > 0.01f)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                transform.parent = collision.transform;
                transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }
        }
    }
}

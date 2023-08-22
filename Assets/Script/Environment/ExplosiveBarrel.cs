using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DestroyableObstacle
{
    public Animator animator;
    public override void TakeDamage(int damage)
    {
        if (!rb.isKinematic)
            return;
        damagedAnimationTimer = Time.time;
        currentHP -= damage;
        if (currentHP < 0)
        {
            AstarPath.active.UpdateGraphs(new Bounds(col.bounds.center, col.bounds.size * 2f));
            Explode();
        }
    }
    private void Explode()
    {
        eventBroadcast.CameraShakeNoti(0.5f, 2f);
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Break");
        col.enabled = false;
        Collider2D[] inZoneObj = Physics2D.OverlapCircleAll(gameObject.transform.position + Vector3.up * 0.3f, 2.5f, LayerMask.GetMask("PlayerHurtBox", "EnemyHurtBox", "Obstacle"));
        foreach (Collider2D obj in inZoneObj)
        {
            if (obj.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager enemy = obj.GetComponent<EnemyStateManager>();
                if (enemy.currentState != enemy.deadState)
                {
                    enemy.GetStun(1f, false);
                    enemy.GetBurn(2);
                    if (enemy.beingControlledBy != null)
                    {
                        enemy.rb.isKinematic = false;
                        enemy.transform.parent = null;
                        Destroy(enemy.beingControlledBy);
                        enemy.beingControlledBy = null;
                        enemy.animator.SetTrigger("Stop");
                        enemy.GetStun(2f, false);
                    }
                    enemy.rb.AddForce((enemy.transform.position - gameObject.transform.position).normalized * 10f, ForceMode2D.Impulse);
                    enemy.TakeDamage(Mathf.Max(0,Mathf.FloorToInt(25*(100-enemy.enemyStat.explosionImmunity)/100f)));
                }
            }
            else if (obj.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager player = obj.GetComponent<PlayerStateManager>();
                player.GetStun(1f);
                player.GetBurn(2);
                player.TakeDamage(20);
                if(player.beingControlledBy != null)
                {
                    player.rb.isKinematic = false;
                    player.transform.parent = null;
                    Destroy(player.beingControlledBy);
                    player.beingControlledBy = null;
                }
                player.rb.AddForce((player.transform.position - gameObject.transform.position).normalized * 10f, ForceMode2D.Impulse);
            }
            else if (obj.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = obj.GetComponent<DestroyableObstacle>();
                if(temp!=null)
                    temp.TakeDamage(100);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.isKinematic || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;
        firstHit = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
        {

            EnemyStateManager enemy = collision.gameObject.GetComponent<EnemyStateManager>();
            enemy.TakeDamage(50);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
        {
            PlayerStateManager player = collision.gameObject.GetComponent<PlayerStateManager>();
            player.TakeDamage(40);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            DestroyableObstacle temp = collision.gameObject.GetComponent<DestroyableObstacle>();
            if(temp != null)
                temp.TakeDamage(100);
        }
        Explode();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.isKinematic || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;
        firstHit = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
        {

            EnemyStateManager enemy = collision.gameObject.GetComponent<EnemyStateManager>();
            enemy.TakeDamage(50);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
        {
            PlayerStateManager player = collision.gameObject.GetComponent<PlayerStateManager>();
            player.TakeDamage(40);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            DestroyableObstacle temp = collision.gameObject.GetComponent<DestroyableObstacle>();
            if(temp != null)
                temp.TakeDamage(100);
        }
        Explode();
    }
}

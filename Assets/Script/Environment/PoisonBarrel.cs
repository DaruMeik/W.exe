using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBarrel : DestroyableObstacle
{
    public GameObject toxic;
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
            ReleaseToxic();
        }
    }
    private void ReleaseToxic()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        GameObject temp = Instantiate(toxic, transform);
        animator.SetTrigger("Break");
        StartCoroutine(Disappear());
        col.enabled = false;
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(12f);
        SelfDestruct();
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
            if (temp != null)
                temp.TakeDamage(100);
        }
        ReleaseToxic();
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
            if (temp != null)
                temp.TakeDamage(100);
        }
        ReleaseToxic();
    }
}

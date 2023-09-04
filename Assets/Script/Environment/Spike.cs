using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D col;
    public PlayerStat playerStat;
    private bool readyToTrigger = true;
    private bool readyToDamage = false;
    public void StopTrigger()
    {
        readyToTrigger = false;
    }
    public void StartTrigger()
    {
        readyToTrigger = true;
        col.enabled = false;
        col.enabled = true;
    }
    public void StopDamage()
    {
        readyToDamage = false;
    }
    public void StartDamage()
    {
        readyToDamage = true;
        col.enabled = false;
        col.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (readyToTrigger && !(collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox") && playerStat.featherStep))
        {
            animator.SetTrigger("Trigger");
        }
        if (readyToDamage)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                temp.TakeDamage(20);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(30);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(25);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}

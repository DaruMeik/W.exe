using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBarrel : DestroyableObstacle
{
    public GameObject toxic;
    public Animator animator;
    public override void TakeDamage(int damage)
    {
        damagedAnimationTimer = Time.time;
        currentHP -= damage;
        if (currentHP < 0)
        {
            GameObject temp = Instantiate(toxic,transform);
            animator.SetTrigger("Break");
            StartCoroutine(Disappear());
            col.enabled = false;
        }
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(12f);
        SelfDestruct();
    }
}

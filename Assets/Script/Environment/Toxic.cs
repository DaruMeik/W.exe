using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxic : MonoBehaviour
{
    private List<GameObject> insideColObjs = new List<GameObject>();
    private List<float> damageStack = new List<float>();
    private float nextTriggerTime = 0;
    private float nextStackTime = 0;
    private bool ready = false;

    private void OnEnable()
    {
        insideColObjs.Clear();
        nextTriggerTime = Time.time;
        nextStackTime = Time.time;
    }
    private void Update()
    {
        if (Time.time >= nextTriggerTime)
        {
            for (int i = 0; i < insideColObjs.Count; i++)
            {
                ready = false;
                if (insideColObjs[i].layer == LayerMask.NameToLayer("PlayerCollision"))
                {
                    insideColObjs[i].GetComponentInParent<PlayerStateManager>().TakeDamage(Mathf.FloorToInt(damageStack[i]), true);
                    damageStack[i] += Mathf.Min(25, Mathf.FloorToInt(damageStack[i]) * 25 / 100f);
                }
                else if (insideColObjs[i].layer == LayerMask.NameToLayer("EnemyHurtBox"))
                {
                    insideColObjs[i].GetComponent<EnemyStateManager>().TakeDamage(Mathf.FloorToInt(damageStack[i]));
                    damageStack[i] += Mathf.Min(15, Mathf.FloorToInt(damageStack[i]) * 25 / 100f);
                }
            }
            ready = true;
            nextTriggerTime = Time.time + 0.25f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        insideColObjs.Add(collision.gameObject);
        damageStack.Add(1);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(RemoveFromList(collision));
    }
    IEnumerator RemoveFromList(Collider2D collision)
    {
        while (!ready)
        {
            yield return new WaitForSeconds(0.1f);
        }
        int temp = insideColObjs.IndexOf(collision.gameObject);
        insideColObjs.RemoveAt(temp);
        damageStack.RemoveAt(temp);
    }
}

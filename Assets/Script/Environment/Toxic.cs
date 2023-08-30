using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxic : MonoBehaviour
{
    private List<GameObject> insideColObjs = new List<GameObject>();
    private float nextTriggerTime = 0;
    private bool ready = false;

    private void OnEnable()
    {
        insideColObjs.Clear();
        nextTriggerTime = Time.time;
    }
    private void Update()
    {
        if (Time.time >= nextTriggerTime)
        {
            for (int i = 0; i < insideColObjs.Count; i++)
            {
                ready = false;
                if (insideColObjs[i].layer == LayerMask.NameToLayer("PlayerHurtBox"))
                {
                    insideColObjs[i].GetComponent<PlayerStateManager>().GetPoison(1);
                }
                else if (insideColObjs[i].layer == LayerMask.NameToLayer("EnemyHurtBox"))
                {
                    EnemyStateManager temp = insideColObjs[i].GetComponent<EnemyStateManager>();
                    temp.GetPoison(1);
                }
            }
            ready = true;
            nextTriggerTime = Time.time + 0.5f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        insideColObjs.Add(collision.gameObject);
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
    }
    public IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}

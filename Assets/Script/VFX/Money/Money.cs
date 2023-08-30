using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value;
    public Transform target;
    public bool ready;
    public Animator animator;
    [SerializeField] private GameObject afterImageVFX;
    [SerializeField] private SpriteRenderer coinSprite;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    public float waitTime = 0f;
    private float afterImageSpawnTime = 0f;

    private void OnEnable()
    {
        ready = false;
        animator.enabled = false;
    }

    private void OnDisable()
    {
        playerStat.money += value;
        eventBroadcast.UpdateMoneyNoti();
    }
    private void Update()
    {
        if (!ready || Time.time < waitTime)
        {
            transform.Translate(Vector2.down * 0.5f * Time.deltaTime);
            return;
        }
        if (!animator.enabled)
        {
            animator.enabled = true;
        }
        if (Time.time > afterImageSpawnTime)
        {
            afterImageSpawnTime = Time.time + 0.05f;
            GameObject temp = GameObject.Instantiate(afterImageVFX);
            temp.transform.position = coinSprite.transform.position;
        }
        transform.right = target.position + Vector3.up * 0.5f - transform.position;
        rb.velocity = 12f * (target.position + Vector3.up*0.5f - transform.position).normalized * Mathf.Max(0.5f, (target.position + Vector3.up * 0.5f - transform.position).magnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}

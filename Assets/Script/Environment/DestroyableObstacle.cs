using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DestroyableObstacle : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Material whiteFlashMat;
    [SerializeField] public Collider2D col;
    public Material defaultMat;
    public Rigidbody2D rb;
    public float flashWhiteTimer = 0;
    public float damagedAnimationTimer = 0;
    public bool show;
    public int currentHP;
    public bool firstHit = true;
    private Vector3 spawnPos;
    public EventBroadcast eventBroadcast;

    private void OnEnable()
    {
        spawnPos = transform.position;
        show = true;
        rb.isKinematic = true;
        firstHit = true;
        defaultMat = spriteRenderer.material;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        currentHP = 50;
        AstarPath.active.UpdateGraphs(new Bounds(col.bounds.center, col.bounds.size * 2f));
    }
    private void OnDisable()
    {
        col.enabled = true;
        if (AstarPath.active != null)
        {
            AstarPath.active.UpdateGraphs(new Bounds(col.bounds.center, col.bounds.size * 2f));
            AstarPath.active.UpdateGraphs(new Bounds(spawnPos, col.bounds.size * 2f));
        }
        col.enabled = false;
    }
    private void Update()
    {
        if (Time.time - damagedAnimationTimer < 0.6f)
        {
            if (Time.time - flashWhiteTimer >= 0.2f)
            {
                if (show)
                {
                    spriteRenderer.material = defaultMat;
                }
                else
                {
                    spriteRenderer.material = whiteFlashMat;
                }
                show = !show;
                flashWhiteTimer = Time.time;
            }
        }
        else
        {
            spriteRenderer.material = defaultMat;
        }
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    public virtual void TakeDamage(int damage)
    {
        if (!rb.isKinematic)
            return;
        damagedAnimationTimer = Time.time;
        currentHP -= damage;
        if (currentHP < 0)
        {
            SelfDestruct();
        }
    }
    public void Throw()
    {
        rb.isKinematic = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.isKinematic || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;
        firstHit = false;
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
        SelfDestruct();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.isKinematic || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;
        firstHit = false;
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
        SelfDestruct();
    }
}

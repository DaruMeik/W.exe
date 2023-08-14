using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int ID = 0;
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public float spawnTime;
    public bool ready = false;
    public int atkPerc;
    public bool firstHit = true;
    public bool isDestroyable;
    public bool isBlockable;
    public bool isPushable;
    public SpriteRenderer[] bulletSprites;
    [SerializeField] public PlayerStat playerStat;
    [SerializeField] public EventBroadcast eventBroadcast;

    // Status effect
    public bool isBurning = false;
    protected virtual void OnEnable()
    {
        ready = false;
        firstHit = true;
        spawnTime = Time.time;
    }
    protected virtual void Start()
    {
        if (bySelf )
        {
            if(playerStat.burningBullet && Random.Range(0, 100) >= 75)
            {
                isBurning = true;
                foreach (SpriteRenderer bulletSprite in bulletSprites)
                {
                    bulletSprite.color = Color.red;
                }
            }
            else
            {
                foreach (SpriteRenderer sprite in bulletSprites)
                {
                    float alpha = sprite.color.a;
                    sprite.color = new Color32(107, 206, 107, 255);
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                }
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in bulletSprites)
            {
                float alpha = sprite.color.a;
                sprite.color = new Color32(206, 107, 107, 255);
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            }
        }
    }
    protected virtual void Update()
    {
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                float attackModifier = 0f;
                if (playerStat.critable && Random.Range(0,100) >= 90)
                    attackModifier += 200;
                if(isBurning)
                    temp.GetBurn(2.5f);
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0,Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
            if(playerStat.sharpBullet && Random.Range(0,100) >= 50)
            {
                firstHit = true;
                return;
            }
            Destroy(gameObject);
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0,Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
            Destroy(gameObject);
        }
    }
}

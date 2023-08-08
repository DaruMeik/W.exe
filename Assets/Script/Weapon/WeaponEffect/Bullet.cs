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
    public SpriteRenderer bulletSprite;
    [SerializeField] public PlayerStat playerStat;

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
        if (bySelf && playerStat.burningBullet && Random.Range(0, 100) > 90)
        {
            isBurning = true;
            bulletSprite.color = Color.red;
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
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
                if(isBurning)
                    temp.GetBurn(6f);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            Destroy(gameObject);
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager temp = collision.GetComponent<PlayerStateManager>();
                temp.TakeDamage(WeaponDatabase.weaponList[ID].power);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            Destroy(gameObject);
        }
    }
}

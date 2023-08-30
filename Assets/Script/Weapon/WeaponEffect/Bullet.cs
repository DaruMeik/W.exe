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
    public int HP = 0;
    public float damagedAnimationTimer = 0;
    public bool isDestroyable;
    public bool isBlockable;
    public bool isPushable;
    public SpriteRenderer[] bulletSprites;
    public GameObject moneyGenerator;
    [SerializeField] public PlayerStat playerStat;
    [SerializeField] public EventBroadcast eventBroadcast;

    [Header("VFX")]
    public GameObject critVFX;

    // Status effect
    public bool isBurning = false;
    public bool isCrit = false;
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
            if(playerStat.fireBullet && WeaponDatabase.weaponList[ID].weaponType == "Gun" && Random.Range(0, 100) < 15)
            {
                isBurning = true;
                foreach (SpriteRenderer bulletSprite in bulletSprites)
                {
                    bulletSprite.color = Color.red;
                }
            }
            else if (playerStat.critableGun && WeaponDatabase.weaponList[ID].weaponType == "Gun" && Random.Range(0, 100) < 10)
            {
                isCrit = true;
                foreach (SpriteRenderer bulletSprite in bulletSprites)
                {
                    bulletSprite.color = Color.yellow;
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
            if (playerStat.stubborn)
                atkPerc += Mathf.FloorToInt((playerStat.maxHP - playerStat.currentHP) * 200 / playerStat.maxHP);
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

    public virtual void TakeDamage(int damage)
    {
        Debug.Log("OUch");
        HP = Mathf.Max(0, HP - damage);
        damagedAnimationTimer = Time.time;
        if (HP <= 0)
        {
            if (bySelf && playerStat.goldBuild)
            {
                Instantiate(moneyGenerator).GetComponent<MoneyGenerator>().GenerateMoney(transform.position, 1);
            }
            Destroy(gameObject);
        }
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
                if (ID == playerStat.currentWeapon[0])
                    attackModifier += playerStat.defaultWeaponAtkUpPerc;
                if (isCrit)
                {
                    attackModifier += 200;
                    Instantiate(critVFX).transform.position = collision.transform.position;
                }
                if(isBurning)
                    temp.GetBurn(1);
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet")
            {
                Bullet temp = collision.GetComponentInParent<Bullet>();
                Debug.Log(temp);
                if(temp != null)
                {
                    if (temp.HP > 0)
                    {
                        float attackModifier = 0f;
                        if (ID == playerStat.currentWeapon[0])
                            attackModifier += playerStat.defaultWeaponAtkUpPerc;
                        if (isCrit)
                        {
                            attackModifier += 200;
                            Instantiate(critVFX).transform.position = collision.transform.position;
                        }
                        temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
                    }
                    else
                    {
                        firstHit = true;
                        return;
                    }
                }
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
            Debug.Log(collision.name);
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
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "PlayerBullet")
            {
                Bullet temp = collision.GetComponentInParent<Bullet>();
                if (temp != null)
                {
                    if (temp.HP > 0)
                    {
                        temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
                    }
                    else
                    {
                        firstHit = true;
                        return;
                    }
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0,Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
            Debug.Log(collision.name);
            Destroy(gameObject);
        }
    }
}

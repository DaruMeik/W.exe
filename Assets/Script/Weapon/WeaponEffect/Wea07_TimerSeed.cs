using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea07_TimerSeed : Bullet
{
    public Collider2D col;
    public Animator animator;
    private bool isBloom = false;
    //VFX
    [SerializeField] private SpriteRenderer flowerSprite;
    [SerializeField] private Material whiteFlashMat;
    private Material defaultMat;
    private float flashWhiteTimer = 0;
    private float nextResetTimer = 0;
    private bool show;
    protected override void OnEnable()
    {
        base.OnEnable();
        flashWhiteTimer = 0;
        damagedAnimationTimer = 0;
        defaultMat = flowerSprite.material;
    }
    protected  override void Update()
    {
        if (!ready)
            return;
        if (Time.time - damagedAnimationTimer < 0.6f)
        {
            if (Time.time - flashWhiteTimer >= 0.1f)
            {
                if (show)
                {
                    flowerSprite.material = defaultMat;
                }
                else
                {
                    flowerSprite.material = whiteFlashMat;
                }
                show = !show;
                flashWhiteTimer = Time.time;
            }
        }
        else if (show && flowerSprite != null)
        {
            flowerSprite.material = defaultMat;
        }
        if (rb.velocity.magnitude > 0.25f && Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Wall")))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            animator.SetTrigger("Trigger");
        }
        if (Time.time > nextResetTimer && isBloom)
        {
            col.enabled = false;
            col.enabled = true;
            nextResetTimer = Time.time + 0.5f;
        }
        if (!bySelf && Time.time - spawnTime > 12f)
        {
            Destroy(gameObject);
        }
    }
    public void Bloom()
    {
        isBloom = true;
        col.enabled = false;
        col.enabled = true;
    }
    public void Explode()
    {
        eventBroadcast.CameraShakeNoti(0.5f, 2f);
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Explode");
        col.enabled = false;
        Collider2D[] inZoneObj = null;
        inZoneObj = Physics2D.OverlapCircleAll(gameObject.transform.position + Vector3.up * 0.3f, 2.5f, LayerMask.GetMask("PlayerHurtBox", "EnemyHurtBox", "Obstacle"));
        foreach (Collider2D obj in inZoneObj)
        {
            if (obj.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager enemy = obj.GetComponent<EnemyStateManager>();
                if (enemy.currentState != enemy.deadState)
                {
                    enemy.GetStun(1f, false);
                    enemy.GetBurn(1);
                    float attackModifier = 0;
                    if (ID == playerStat.currentWeapon[0])
                        attackModifier += playerStat.defaultWeaponAtkUpPerc;
                    if (playerStat.critableGun && Random.Range(0, 100) > 90)
                    {
                        attackModifier += 200;
                        Instantiate(critVFX).transform.position = enemy.transform.position;
                    }
                    enemy.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
                    if (enemy.beingControlledBy != null)
                    {
                        enemy.rb.isKinematic = false;
                        enemy.transform.parent = null;
                        Destroy(enemy.beingControlledBy);
                        enemy.beingControlledBy = null;
                        enemy.animator.SetTrigger("Stop");
                        enemy.GetStun(2f, false);
                    }
                    enemy.rb.AddForce((enemy.transform.position - gameObject.transform.position).normalized * 5f, ForceMode2D.Impulse);
                }
            }
            else if (obj.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                PlayerStateManager player = obj.GetComponent<PlayerStateManager>();
                player.GetStun(1f);
                player.GetBurn(1);
                player.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
                if (player.beingControlledBy != null)
                {
                    player.rb.isKinematic = false;
                    player.transform.parent = null;
                    Destroy(player.beingControlledBy);
                    player.beingControlledBy = null;
                }
                player.rb.AddForce((player.transform.position - gameObject.transform.position).normalized * 5f, ForceMode2D.Impulse);
            }
            else if (obj.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = obj.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
        }
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || !firstHit || !isBloom || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.tag == "Low")
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            firstHit = false;
            animator.SetTrigger("Trigger");
            if(rb.velocity.magnitude > 0.01f)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                transform.parent = collision.transform;
                transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            firstHit = false;
            animator.SetTrigger("Trigger");
            if (rb.velocity.magnitude > 0.01f)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                transform.parent = collision.transform;
                transform.localPosition = new Vector3(0f, 0.5f, 0f);
            }
        }
    }
}

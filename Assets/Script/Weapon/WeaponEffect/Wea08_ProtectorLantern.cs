using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea08_ProtectorLantern : Bullet
{
    public GameObject[] spawnedShield;
    public int activeShield;
    public GameObject laternLight;
    //VFX
    private SpriteRenderer shieldSprite;
    [SerializeField] private Material whiteFlashMat;
    private Material defaultMat;
    private float flashWhiteTimer = 0;
    private bool show;

    protected override void OnEnable()
    {
        base.OnEnable();
        flashWhiteTimer = 0;
        damagedAnimationTimer = 0;
    }
    protected override void Update()
    {
        if (!ready)
            return;
        if (Time.time - damagedAnimationTimer < 0.6f)
        {
            if (Time.time - flashWhiteTimer >= 0.1f)
            {
                if (show)
                {
                    shieldSprite.material = defaultMat;
                }
                else
                {
                    shieldSprite.material = whiteFlashMat;
                }
                show = !show;
                flashWhiteTimer = Time.time;
            }
        }
        else if (show && shieldSprite != null)
        {
            shieldSprite.material = defaultMat;
        }
        if (!bySelf && Time.time - spawnTime > 5f)
            Destroy(gameObject);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready)
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet")
            {
                Bullet temp = collision.GetComponent<Bullet>();
                if (temp != null && temp.isBlockable)
                    temp.atkPerc = -1000;
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "PlayerBullet")
            {
                Bullet temp = collision.GetComponent<Bullet>();
                if (temp != null && temp.isBlockable)
                    temp.atkPerc = -1000;
            }
        }
    }
    public void TurnOnShield()
    {
        spawnedShield[activeShield].SetActive(true);
        spawnedShield[activeShield].tag = gameObject.tag;
        shieldSprite = spawnedShield[activeShield].GetComponent<SpriteRenderer>();
        defaultMat = shieldSprite.material;
    }
}

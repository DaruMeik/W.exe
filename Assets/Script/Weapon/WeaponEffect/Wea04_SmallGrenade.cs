using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea04_SmallGrenade : Bullet
{
    public Vector3 endPoint;
    public Collider2D col;
    public GameObject fireObj;
    public GameObject warningTile;
    public GameObject theBomb;
    protected override void OnEnable()
    {
        base.OnEnable();
        col.enabled = false;
    }
    private void OnDisable()
    {
        if (warningTile != null)
            Destroy(warningTile);
    }
    protected override void Update()
    {
        if (!ready)
            return;
    }
    public void FinishAnimation()
    {
        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy()
    {
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        theBomb.SetActive(false);
        col.enabled = true;
        fireObj.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        warningTile.transform.parent = gameObject.transform;
        Destroy(gameObject);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || collision.tag == "Low")
            return;
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
                if (isBurning)
                    temp.GetBurn(1);
                temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc + attackModifier) / 100f)));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.tag == "EnemyBullet")
            {
                Bullet temp = collision.GetComponentInParent<Bullet>();
                if (temp != null)
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
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
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
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.Max(0, Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f)));
            }
        }
    }
}
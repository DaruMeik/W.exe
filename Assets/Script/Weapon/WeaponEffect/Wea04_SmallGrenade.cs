using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea04_SmallGrenade : MonoBehaviour
{
    public int ID = 4;
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector3 endPoint;
    public Collider2D col;
    public GameObject fireObj;
    public GameObject warningTile;
    public GameObject theBomb;
    public bool ready = false;
    public int atkPerc;
    private void OnEnable()
    {
        ready = false;
        col.enabled = false;
    }
    private void Update()
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
        rb.MovePosition(endPoint);
        yield return new WaitForSeconds(0.2f);
        theBomb.SetActive(false);
        col.enabled = true;
        fireObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        warningTile.transform.parent = gameObject.transform;
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.tag == "Low")
            return;
        if (bySelf && collision.gameObject.layer != LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                DestroyableObstacle temp = collision.GetComponent<DestroyableObstacle>();
                if (temp != null)
                    temp.TakeDamage(Mathf.FloorToInt(WeaponDatabase.weaponList[ID].power * (100 + atkPerc) / 100f));
            }
        }
        else if (!bySelf && collision.gameObject.layer != LayerMask.NameToLayer("EnemyHurtBox"))
        {
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
        }
    }
}
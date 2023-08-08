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
        rb.MovePosition(endPoint);
        yield return new WaitForSeconds(0.2f);
        theBomb.SetActive(false);
        col.enabled = true;
        fireObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        warningTile.transform.parent = gameObject.transform;
        Destroy(gameObject);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.tag == "Low")
            return;
        base.OnTriggerEnter2D(collision);
    }
}
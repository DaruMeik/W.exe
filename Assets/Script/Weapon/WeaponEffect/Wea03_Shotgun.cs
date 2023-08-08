using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea03_Shotgun : Bullet
{
    protected override void Update()
    {
        if (!ready)
            return;
        if (Vector2.Distance(spawnPos, transform.position) > 6f)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || !firstHit || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.tag == "Low")
            return;
        base.OnTriggerEnter2D(collision);
    }
}
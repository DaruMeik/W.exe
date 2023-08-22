using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMail : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool bySelf;
    public Vector2 spawnPos;
    public bool isPickupable = false;
    public bool ready = false;
    public PlayerStat playerStat;
    private bool firstHit = true;
    private bool flyingBack = false;
    public Transform player;
    public GameObject shockWave;
    public EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        isPickupable = false;
        ready = false;
        firstHit = true;
        flyingBack = false;
        eventBroadcast.sendingCard += SelfDestruct;
    }
    private void OnDisable()
    {
        eventBroadcast.sendingCard -= SelfDestruct;
    }
    private void Update()
    {
        if (!bySelf && Vector2.Distance(spawnPos, rb.position) < 0.75f)
        {
            transform.Translate(Vector2.down * Time.deltaTime);
        }
        if (!ready)
            return;
        if (Vector2.Distance(spawnPos, transform.position) > 20f)
        {
            isPickupable = true;
            rb.velocity = Vector2.zero;
        }
        if (!flyingBack && isPickupable && PlayerControl.Instance.pInput.Player.Throw.IsPressed() && playerStat.cardReadyPerc != 100)
        {
            flyingBack = true;
        }
        if (flyingBack)
        {
            Vector3 lookDir = player.position - transform.position;
            float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 15f);
            rb.velocity = 12f * (player.position - transform.position).normalized * Mathf.Max(0.5f, (player.position - transform.position).magnitude);
        }
    }
    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ready || (!firstHit && !isPickupable) || collision.gameObject.layer == LayerMask.NameToLayer("Bullet") || collision.tag == "Low")
            return;
        if (!isPickupable && collision.tag != "Player")
        {
            firstHit = false;
            if (playerStat.cardShockWave)
            {
                GameObject temp = Instantiate(shockWave, transform);
                temp.transform.parent = null;
            }
            if (new List<int> { LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Obstacle") }.Contains(collision.gameObject.layer))
            {
                isPickupable = true;
                rb.velocity = Vector2.zero;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
            {
                EnemyStateManager temp = collision.GetComponent<EnemyStateManager>();
                temp.marked = true;
                temp.mark.SetActive(true);
                temp.TakeDamage(Mathf.FloorToInt((WeaponDatabase.fishingMail.power + playerStat.extraCardDamage) * (100 + playerStat.atkPerc) / 100f));
                Destroy(gameObject);
            }
        }
        else if (isPickupable && collision.tag == "Player")
        {
            firstHit = false;
            playerStat.cardReadyPerc = 100;
            playerStat.eventBroadcast.UpdateCardUINoti();
            Destroy(gameObject);
        }
    }
}

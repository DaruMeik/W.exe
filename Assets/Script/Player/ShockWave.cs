using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    private void OnEnable()
    {
        transform.localScale = Vector3.one * playerStat.shockWaveRange * 2f;
    }
    public void TurnOff()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" && collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            GameObject.Destroy(collision.gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
        {
            EnemyStateManager temp = collision.gameObject.GetComponent<EnemyStateManager>();
            temp.GetStun(playerStat.shockWaveStunTime, true);
            if (playerStat.shockwaveDealDamage)
            {
                temp.TakeDamage(Mathf.FloorToInt((WeaponDatabase.fishingMail.power + playerStat.extraCardDamage) * (100 + playerStat.atkPerc) / 100f));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Collider2D col;
    private void OnEnable()
    {
        transform.localScale = Vector3.one * playerStat.shockWaveRange * 2f;
    }
    public void TurnOff()
    {
        col.enabled = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" && collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            GameObject.Destroy(collision.gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtBox"))
        {
            collision.gameObject.GetComponent<EnemyStateManager>().GetStun(playerStat.shockWaveStunTime);
        }
    }
}

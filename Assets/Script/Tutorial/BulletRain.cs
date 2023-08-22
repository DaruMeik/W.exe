using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRain : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private GameObject empty;
    private void OnEnable()
    {
        StartCoroutine(spamBullet());
    }

    IEnumerator spamBullet()
    {
        while (true)
        {
            WeaponDatabase.tutorialSpecialBullet.weaponBaseEffect.ApplyEffect(transform.position, new Vector3(transform.position.x, -transform.position.y, transform.position.z), false, playerStat, null, ref empty);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea00_BusterEffect : WeaponBaseEffect
{
    private int weaponId = 0;
    float speed = 12f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat = null, float chargedAmount = 0)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
        instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
        // Rotate Skill
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        //Set up Obj
        instancedObj.transform.position = startPoint;
        Wea00_Buster temp = instancedObj.GetComponent<Wea00_Buster>();
        temp.spawnPos = startPoint;
        temp.bySelf = bySelf;
        temp.rb.velocity = (endPoint - startPoint).normalized * speed;

        if (bySelf)
        {
            temp.atkPerc = playerStat.atkPerc;
        }
        else
        {
            temp.atkPerc = 0;
        }

        temp.ready = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea01_GatlingEffect : WeaponBaseEffect
{
    private int weaponId = 1;
    float speed = 10f;
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
        Wea01_Gatling temp = instancedObj.GetComponent<Wea01_Gatling>();
        temp.spawnPos = startPoint;
        temp.bySelf = bySelf;
        if(bySelf)
            temp.rb.velocity = (endPoint - startPoint).normalized * speed;
        else
            temp.rb.velocity = (endPoint - startPoint).normalized * speed * 0.75f;
        temp.ready = true;
    }
}

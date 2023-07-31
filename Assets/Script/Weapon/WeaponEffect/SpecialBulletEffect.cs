using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBulletEffect : WeaponBaseEffect
{
    private int weaponId = 0;
    float speed = 10f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.tutorialSpecialBullet.weaponHitBox) as GameObject;
        instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
        // Rotate Skill
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        //Set up Obj
        instancedObj.transform.position = startPoint;
        SpecialBullet temp = instancedObj.GetComponent<SpecialBullet>();
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
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

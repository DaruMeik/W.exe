using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea09_PaintGunEffect : WeaponBaseEffect
{
    private int weaponId = 9;
    float speed = 8f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
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
        Wea09_PaintGun temp = instancedObj.GetComponent<Wea09_PaintGun>();
        temp.spawnPos = startPoint;
        temp.bySelf = bySelf;
        if (bySelf)
        {
            temp.rb.velocity = (endPoint - startPoint).normalized * speed;
            temp.atkPerc = playerStat.atkPerc;
        }
        else
        {
            temp.rb.velocity = (endPoint - startPoint).normalized * speed;
            temp.atkPerc = 0;
        }
        temp.ready = true;
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

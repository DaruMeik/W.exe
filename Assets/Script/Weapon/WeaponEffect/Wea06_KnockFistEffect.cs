using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea06_KnockFistEffect : WeaponBaseEffect
{
    private int weaponId = 6;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
        // Rotate Skill
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        //Set up Obj
        instancedObj.transform.position = startPoint + (endPoint - startPoint).normalized * 1.2f;
        Wea06_KnockFist temp = instancedObj.GetComponent<Wea06_KnockFist>();
        temp.dir = (endPoint - startPoint).normalized;
        temp.bySelf = bySelf;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMailEffect : WeaponBaseEffect
{
    float speed = 20f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.fishingMail.weaponHitBox) as GameObject;
        // Rotate Skill
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 15f);
        //Set up Obj
        instancedObj.transform.position = startPoint + lookDir.normalized * 0.5f;
        FishingMail temp = instancedObj.GetComponent<FishingMail>();
        temp.spawnPos = startPoint;
        temp.bySelf = bySelf;
        if (bySelf)
        {
            temp.rb.velocity = (endPoint - startPoint).normalized * speed;
        }
        else
        {
            temp.rb.velocity = Vector2.zero;
            temp.isPickupable = true;
        }
        temp.playerStat = playerStat;
        temp.ready = true;
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

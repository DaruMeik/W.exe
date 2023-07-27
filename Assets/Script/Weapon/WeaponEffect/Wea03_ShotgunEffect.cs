using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea03_ShotgunEffect : WeaponBaseEffect
{
    private int weaponId = 3;
    float speed = 12f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        for (int i = 0; i < 5; i++)
        {
            GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
            instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
            // Rotate Skill
            Vector3 lookDir = endPoint - startPoint;
            float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f - 15f + 7.5f * i;
            instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            //Set up Obj
            instancedObj.transform.position = startPoint;
            Wea03_Shotgun temp = instancedObj.GetComponent<Wea03_Shotgun>();
            temp.spawnPos = startPoint;
            temp.bySelf = bySelf;
            if (bySelf)
            {
                temp.rb.velocity = temp.rb.transform.right.normalized * speed;
                temp.atkPerc = playerStat.atkPerc;
            }
            else
            {
                temp.rb.velocity = temp.rb.transform.right.normalized * speed * 0.75f;
                temp.atkPerc = 0;
            }
            temp.ready = true;
        }
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}
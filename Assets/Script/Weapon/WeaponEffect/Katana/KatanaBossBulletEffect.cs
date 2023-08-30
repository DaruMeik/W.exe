using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kata00_WideSlashEffect : WeaponBaseEffect
{
    private int weaponId = 0;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.kataWeaponList[weaponId].weaponHitBox) as GameObject;
        // Rotate Skill
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        //Set up Obj
        instancedObj.transform.position = startPoint + (endPoint - startPoint).normalized * 2.5f;
        Kata00_WideSlash temp = instancedObj.GetComponent<Kata00_WideSlash>();
        temp.spawnPos = startPoint;
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

public class Kata01_ShockWaveEffect : WeaponBaseEffect
{
    private int weaponId = 1;
    float speed = 12f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.kataWeaponList[weaponId].weaponHitBox) as GameObject;
        instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
        // Rotate Skill
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        //Set up Obj
        instancedObj.transform.position = startPoint;
        Kata01_ShockWave temp = instancedObj.GetComponent<Kata01_ShockWave>();
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
public class Kata02_ThriceWaveEffect : WeaponBaseEffect
{
    private int weaponId = 1;
    float speed = 12f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        for (int i = 0; i < 3; i++)
        {
            GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.kataWeaponList[weaponId].weaponHitBox) as GameObject;
            instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
            // Rotate Skill
            Vector3 lookDir = endPoint - startPoint;
            float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f - 45f + 22.5f * i;
            instancedObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            //Set up Obj
            instancedObj.transform.position = startPoint;
            Kata01_ShockWave temp = instancedObj.GetComponent<Kata01_ShockWave>();
            temp.spawnPos = startPoint;
            temp.bySelf = bySelf;
            temp.rb.velocity = temp.rb.transform.right.normalized * speed;
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
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

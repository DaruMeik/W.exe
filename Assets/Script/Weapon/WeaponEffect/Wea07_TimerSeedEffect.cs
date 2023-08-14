using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea07_TimerSeedEffect : WeaponBaseEffect
{
    private int weaponId = 7;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
        instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
        // Rotate Skill
        //Set up Obj
        Wea07_TimerSeed temp = instancedObj.GetComponent<Wea07_TimerSeed>();
        temp.spawnPos = startPoint;
        temp.bySelf = bySelf;
        if (bySelf)
        {
            instancedObj.transform.position = endPoint;
            temp.atkPerc = playerStat.atkPerc;
            if (playerStat.BEEG)
                instancedObj.transform.localScale *= 1.5f;
        }
        else
        {
            instancedObj.transform.position = endPoint + Random.onUnitSphere * 0.5f;
            temp.atkPerc = 0;
        }

        temp.ready = true;
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

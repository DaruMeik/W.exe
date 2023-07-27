using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea04_SmallGrenadeEffect : WeaponBaseEffect
{
    private int weaponId = 4;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;

        //Set up Obj
        instancedObj.transform.position = startPoint;
        instancedObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
        Wea04_SmallGrenade temp = instancedObj.GetComponent<Wea04_SmallGrenade>();
        temp.bySelf = bySelf;
        temp.warningTile.transform.parent = null;
        temp.warningTile.transform.position = endPoint;
        temp.warningTile.SetActive(true);
        temp.endPoint = endPoint;
        temp.rb.velocity = (endPoint - startPoint) / 1.2f;

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
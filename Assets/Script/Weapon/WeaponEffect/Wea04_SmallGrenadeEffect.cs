using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea04_SmallGrenadeEffect : WeaponBaseEffect
{
    private int weaponId = 4;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;

        //Set up Obj
        instancedObj.transform.position = startPoint;
        Wea04_SmallGrenade temp = instancedObj.GetComponent<Wea04_SmallGrenade>();
        temp.bySelf = bySelf;
        temp.warningTile.SetActive(true);
        temp.endPoint = endPoint;
        temp.rb.velocity = (endPoint - startPoint)* Application.targetFrameRate / (60);

        if (bySelf)
        {
            temp.atkPerc = playerStat.atkPerc;
            if (playerStat.BEEGBomb)
                instancedObj.transform.localScale *= 1.5f;
        }
        else
        {
            temp.atkPerc = 0;
            if(userRigid.GetComponent<EnemyStateManager>().enemyStat.enemyType == "Boss" || userRigid.GetComponent<EnemyStateManager>().enemyStat.enemyType == "Miniboss")
                instancedObj.transform.localScale *= 1.5f;
        }
        temp.warningTile.transform.parent = null;
        temp.warningTile.transform.position = endPoint;
        temp.ready = true;
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}
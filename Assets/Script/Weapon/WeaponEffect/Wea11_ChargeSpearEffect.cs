using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea11_ChargeSpearEffect : WeaponBaseEffect
{

    private int weaponId = 11;
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
        instancedObj.transform.position = startPoint;
        userRigid.gameObject.transform.parent = instancedObj.transform;
        Wea11_ChargeSpear temp = instancedObj.GetComponent<Wea11_ChargeSpear>();
        userRigid.isKinematic = true;
        temp.userRB = userRigid;
        RaycastHit2D result = Physics2D.CapsuleCast(startPoint + (endPoint - startPoint).normalized * 0.5f, new Vector2(0.65f,1.25f), CapsuleDirection2D.Vertical, 0f, (endPoint-startPoint).normalized, Mathf.Infinity, LayerMask.GetMask("Wall"));
        temp.endPoint = result.point;
        temp.spawnPos = startPoint;
        temp.bySelf = bySelf;
        if (bySelf)
        {
            temp.playerState = userRigid.gameObject.GetComponent<PlayerStateManager>();
            temp.atkPerc = playerStat.atkPerc;
        }
        else
        {
            temp.enemyState = userRigid.gameObject.GetComponent<EnemyStateManager>();
            temp.atkPerc = 0;
        }
        temp.ready = true;
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

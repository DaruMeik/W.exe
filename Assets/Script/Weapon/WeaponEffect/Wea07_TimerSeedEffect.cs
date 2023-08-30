using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea07_TimerSeedEffect : WeaponBaseEffect
{
    private int weaponId = 7;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
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
            float HPModifier = 0f;
            if (playerStat.sturdyBuild)
            {
                HPModifier += 100;
            }
            temp.HP = Mathf.CeilToInt(35 * (100 + HPModifier)/100f);
            instancedObj.transform.position = startPoint - new Vector3(0f, 0.5f, 0f);
            temp.atkPerc = playerStat.atkPerc;
        }
        else
        {
            temp.HP = 25;
            Vector2 aimPos = endPoint + (Vector3)Random.insideUnitCircle * 2.5f;
            if (!Physics2D.OverlapPoint(aimPos, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D[] results = Physics2D.RaycastAll(startPoint, aimPos - (Vector2)startPoint, (aimPos - (Vector2)startPoint).magnitude, LayerMask.GetMask("Wall"));
                if(results.Length > 0)
                    aimPos = results[results.Length - 1].point;
            }
            instancedObj.transform.position = aimPos;
            temp.atkPerc = 0;
        }

        temp.ready = true;
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

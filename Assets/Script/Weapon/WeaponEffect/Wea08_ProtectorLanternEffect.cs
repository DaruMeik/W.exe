using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea08_ProtectorLanternEffect : WeaponBaseEffect
{

    private int weaponId = 8;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;
        Collider2D[] results;
        if (bySelf)
        {
            results = Physics2D.OverlapCircleAll(startPoint, 10f, LayerMask.GetMask("PlayerCollision"));
        }
        else
        {
            results = Physics2D.OverlapCircleAll(startPoint, 10f, LayerMask.GetMask("EnemyHurtBox"));
        }

        foreach(Collider2D col in results)
        {
            Wea08_ProtectorLantern[] test = col.gameObject.GetComponentsInChildren<Wea08_ProtectorLantern>();
            if(test != null && test.Length > 0)
            {
                foreach(Wea08_ProtectorLantern shield in test)
                {
                    GameObject.Destroy(shield.gameObject);
                }
            }
            GameObject instancedObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
            // Rotate Skill
            Vector3 lookDir = endPoint - col.gameObject.transform.position;
            float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
            //Set up Obj
            instancedObj.transform.parent = col.gameObject.transform;
            instancedObj.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            Wea08_ProtectorLantern temp = instancedObj.GetComponent<Wea08_ProtectorLantern>();
            temp.rb.isKinematic = true;
            if ((lookAngle >= 225f)||((lookAngle >= -135f) && (lookAngle < -45f)))
            {
                temp.activeShield = 0;
                temp.laternLight.transform.rotation = Quaternion.Euler(0f, 0f, -180f);
            }
            else if ((lookAngle >= -45f) && (lookAngle < 45f))
            {
                temp.activeShield = 1;
                temp.laternLight.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if ((lookAngle >= 45f) && (lookAngle < 135f))
            {
                temp.activeShield = 2;
                temp.laternLight.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if ((lookAngle >= 135f && lookAngle < 225f))
            {
                temp.activeShield = 3;
                temp.laternLight.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            temp.spawnPos = startPoint;
            temp.bySelf = bySelf;
            if (bySelf)
            {
                temp.atkPerc = playerStat.atkPerc;
                temp.HP = WeaponDatabase.weaponList[temp.ID].power;
                if (playerStat.BEEG)
                    instancedObj.transform.localScale *= 1.5f;
            }
            else
            {
                temp.atkPerc = 0;
                temp.HP = Mathf.FloorToInt(WeaponDatabase.weaponList[temp.ID].power*0.75f);
            }
            temp.ready = true;
        }
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj) { }
}

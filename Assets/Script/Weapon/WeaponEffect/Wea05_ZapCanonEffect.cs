using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea05_ZapCanonEffect : WeaponBaseEffect
{
    private int weaponId = 5;
    float speed = 20f;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        if (spawnObj == null)
        {
            spawnObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
            spawnObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
            // Rotate Skill
            Vector3 lookDir = endPoint - startPoint;
            float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
            //Set up Obj
            Wea05_ZapCanon temp = spawnObj.GetComponent<Wea05_ZapCanon>();
            temp.rb.isKinematic = true;
            spawnObj.transform.parent = weaponPoint;
            spawnObj.transform.localPosition = new Vector3(1f, 0f, 0f);
            temp.chargeAmount = 20;
            spawnObj.transform.localScale = Vector3.one * (100 + temp.chargeAmount) / 100f;
            temp.bySelf = bySelf;
        }
        else
        {
            Wea05_ZapCanon temp = spawnObj.GetComponent<Wea05_ZapCanon>();
            if(bySelf && playerStat.fasterCharge)
                temp.chargeAmount = Mathf.Min(150, temp.chargeAmount + 12.5f);
            else
                temp.chargeAmount = Mathf.Min(150, temp.chargeAmount + 10);
            spawnObj.transform.localScale = Vector3.one * (100 + temp.chargeAmount) / 100f;
        }
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        if (spawnObj == null)
            return;
        spawnObj.transform.parent = null;
        Wea05_ZapCanon temp = spawnObj.GetComponent<Wea05_ZapCanon>();
        temp.rb.isKinematic = false;
        temp.spawnPos = spawnObj.transform.position;
        if (bySelf)
        {
            temp.rb.velocity = (endPoint - startPoint).normalized * speed;
            temp.atkPerc = playerStat.atkPerc;
            if (playerStat.BEEG)
                temp.gameObject.transform.localScale *= 1.5f;
        }
        else
        {
            temp.rb.velocity = (endPoint - (Vector3)temp.spawnPos).normalized * speed * 0.6f;
            temp.atkPerc = 0;
        }
        temp.ready = true;
        temp.GetComponent<Collider2D>().enabled = false;
        temp.GetComponent<Collider2D>().enabled = true;
        spawnObj = null;
    }
}
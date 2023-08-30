using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wea12_KatanaEffect : WeaponBaseEffect
{
    private int weaponId = 12;
    public override void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj)
    {
        startPoint.z = 0;
        endPoint.z = 0;

        if (spawnObj == null)
        {
            if (bySelf)
                Time.timeScale = Mathf.Max(0, Time.timeScale - 0.5f);
            spawnObj = GameObject.Instantiate(WeaponDatabase.weaponList[weaponId].weaponHitBox) as GameObject;
            spawnObj.tag = (bySelf) ? "PlayerBullet" : "EnemyBullet";
            // Rotate Skill
            //Set up Obj
            Wea12_Katana temp = spawnObj.GetComponent<Wea12_Katana>();
            temp.rb.isKinematic = true;
            spawnObj.transform.parent = weaponPoint;
            spawnObj.transform.localPosition = new Vector3(0f, 0f, 0f);
            if (bySelf)
            {
                Debug.Log(spawnObj.transform.parent.name);
                spawnObj.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                Vector3 lookDir = endPoint - startPoint;
                float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
                spawnObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            }
            temp.chargeAmount = 10;
            spawnObj.transform.localScale = new Vector3((100 + 2 * temp.chargeAmount) / 100f, 1f, 1f);
            temp.bySelf = bySelf;
            if (bySelf)
            {
                temp.playerState = userRigid.gameObject.GetComponent<PlayerStateManager>();
            }
            else
            {
                temp.enemyState = userRigid.gameObject.GetComponent<EnemyStateManager>();
            }
        }
        else
        {
            Wea12_Katana temp = spawnObj.GetComponent<Wea12_Katana>();
            if ((bySelf && playerStat.fasterCharge))
                temp.chargeAmount = Mathf.Min(200, temp.chargeAmount + 25f);
            else if (!bySelf)
                temp.chargeAmount = Mathf.Min(200, temp.chargeAmount + 50f);
            else
                temp.chargeAmount = Mathf.Min(200, temp.chargeAmount + 20);
            if (!bySelf)
            {
                Vector3 lookDir = endPoint - startPoint;
                float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
                spawnObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            }
            spawnObj.transform.localScale = new Vector3((100 + 3 * temp.chargeAmount) / 100f, 1f, 1f);
        }
    }
    public override void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj)
    {
        if (spawnObj == null)
            return;
        if (bySelf)
            Time.timeScale = Mathf.Min(1f, Time.timeScale + 0.5f);
        spawnObj.transform.parent = null;
        spawnObj.transform.position = startPoint;
        Vector3 lookDir = endPoint - startPoint;
        float lookAngle = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        spawnObj.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        Wea12_Katana temp = spawnObj.GetComponent<Wea12_Katana>();
        temp.rb.isKinematic = false;
        temp.spawnPos = spawnObj.transform.position;
        temp.endPoint = endPoint;
        temp.Teleport();
        temp.ready = true;
        spawnObj = null;
    }
}

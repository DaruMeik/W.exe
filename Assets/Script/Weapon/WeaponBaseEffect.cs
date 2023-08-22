using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBaseEffect
{
    public Transform weaponPoint;
    public abstract void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, Rigidbody2D userRigid, ref GameObject spawnObj);
    public abstract void Release(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, ref GameObject spawnObj);
}

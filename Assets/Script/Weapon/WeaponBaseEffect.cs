using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBaseEffect
{
    public abstract void ApplyEffect(Vector3 startPoint, Vector3 endPoint, bool bySelf, PlayerStat playerStat, float chargeAmount = 0);
}

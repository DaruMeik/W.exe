using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Money : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.money += 100;
        playerStat.eventBroadcast.UpdateMoneyNoti();
    }
}

#region HP
public class Upgrade_HP1 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.maxHP += 25;
        playerStat.currentHP += 25;
        playerStat.eventBroadcast.UpdateHPNoti();
    }
}
public class Upgrade_HP2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.maxHP += 50;
        playerStat.currentHP += 50;
        playerStat.eventBroadcast.UpdateHPNoti();
    }
}
public class Upgrade_HP3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.maxHP += 100;
        playerStat.currentHP += 100;
        playerStat.eventBroadcast.UpdateHPNoti();
    }
}
#endregion

#region shock wave
public class Upgrade_ShockWaveRange1 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveRange += 0.5f;
    }
}
public class Upgrade_ShockWaveRange2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveRange += 1f;
    }
}
public class Upgrade_ShockWaveRange3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveRange += 1.5f;
    }
}
public class Upgrade_ShockWaveStunt1 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStuntTime += 0.25f;
    }
}
public class Upgrade_ShockWaveStunt2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStuntTime += 0.5f;
    }
}
public class Upgrade_ShockWaveStunt3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStuntTime += 1f;
    }
}
#endregion

#region movement
public class Upgrade_ExtraDash : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.maxDash += 1;
    }
}
public class Upgrade_MovementSpeed : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.playerSpeed += 1f;
    }
}
#endregion
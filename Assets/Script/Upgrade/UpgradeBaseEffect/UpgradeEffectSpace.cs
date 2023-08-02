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
public class Upgrade_ShockWaveStun1 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStunTime += 0.25f;
    }
}
public class Upgrade_ShockWaveStun2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStunTime += 0.5f;
    }
}
public class Upgrade_ShockWaveStun3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStunTime += 1f;
    }
}
public class Upgrade_ShockWaveDamage : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockwaveDealDamage = true;
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

#region card related
public class Upgrade_CardDamage1: UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraCardDamage += 10;
    }
}
public class Upgrade_CardDamage2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraCardDamage += 20;
    }
}
public class Upgrade_CardDamage3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraCardDamage += 40;
    }
}
public class Upgrade_CardShockWave : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.cardShockWave = true;
    }
}
#endregion

#region possessing related
public class Upgrade_WeaponAmmo1 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraAmmoPerc += 25;
    }
}
public class Upgrade_WeaponAmmo2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraAmmoPerc += 75;
    }
}
public class Upgrade_WeaponAmmo3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraAmmoPerc += 150;
    }
}
public class Upgrade_PHealing1 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraPossessHealingPerc += 20;
    }
}
public class Upgrade_PHealing2 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraPossessHealingPerc += 50;
    }
}
public class Upgrade_PHealing3 : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraPossessHealingPerc += 80;
    }
}
#endregion
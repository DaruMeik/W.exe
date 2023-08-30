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
public class Upgrade_Rage : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.rage = true;
    }
}
#endregion

#region red stuffs
public class Upgrade_DoubleEdge : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.defaultAtkPerc += 20;
        playerStat.atkPerc += 20;
        playerStat.defaultDefPerc -= 20;
        playerStat.defPerc -= 20;
    }
}
public class Upgrade_CloseCombat : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.closeCombat = true;
    }
}
public class Upgrade_SharpShooter : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.sharpShooter = true;
    }
}
public class Upgrade_Stubborn : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.stubborn = true;
    }
}
#endregion

#region green stuffs
public class Upgrade_QuickShot : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraAtkSpeedPerc += 25;
    }
}
public class Upgrade_SwiftMovement : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.swiftMovement = true;
    }
}
public class Upgrade_FeatherStep : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.featherStep = true;
    }
}
public class Upgrade_ShadowMovement : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shadowMovement = true;
    }
}
#endregion

#region blue stuffs
public class Upgrade_MovingFort : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.movingFort = true;
    }
}
public class Upgrade_PoisonResistance : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.poisonResistance = true;
    }
}
public class Upgrade_FireResistance : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.fireResistance = true;
    }
}
public class Upgrade_ShockArmor : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockArmor = true;
    }
}
public class Upgrade_ShockBlast : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockBlast = true;
    }
}
#endregion

#region shock wave
public class Upgrade_ShockWaveRange : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveRange += 1f;
    }
}
public class Upgrade_ShockWaveStun : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.shockWaveStunTime += 0.75f;
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
public class Upgrade_CardShockWave : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.cardShockWave = true;
    }
}
#endregion

#region possessing related
public class Upgrade_WeaponAmmo : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraAmmoPerc += 100;
    }
}
public class Upgrade_PHealing : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.extraPossessHealingPerc += 100;
    }
}
#endregion

#region bullet mod
public class Upgrade_BurnBullet : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.fireBullet = true;
    }
}
public class Upgrade_FasterCharge : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.fasterCharge = true;
    }
}
public class Upgrade_UnseenBlade : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.unseenBlade = true;
    }
}
public class Upgrade_BEEGBomb : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.BEEGBomb = true;
    }
}
public class Upgrade_CritBullet : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.critableGun = true;
    }
}
public class Upgrade_ReflectSword : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.reflectSword = true;
    }
}
public class Upgrade_SharpBullet : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.sharpBullet = true;
    }
}
public class Upgrade_WildCard : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.wildCard = true;
    }
}
public class Upgrade_SturdyBuild : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.sturdyBuild = true;
    }
}
public class Upgrade_GoldBuild : UpgradeBaseEffect
{
    public override void ApplyEffect(PlayerStat playerStat)
    {
        playerStat.goldBuild = true;
    }
}
#endregion
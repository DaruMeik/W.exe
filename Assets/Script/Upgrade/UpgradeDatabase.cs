using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour
{
    public static List<Upgrade> redUpgradeList = new List<Upgrade>();
    public Sprite[] redUpgradeSprite;
    public static List<Upgrade> greenUpgradeList = new List<Upgrade>();
    public Sprite[] greenUpgradeSprite;
    public static List<Upgrade> blueUpgradeList = new List<Upgrade>();
    public Sprite[] blueUpgradeSprite;
    public static List<Upgrade> bulletModList = new List<Upgrade>();
    public Sprite[] bulletModSprite;
    private void Awake()
    {
        redUpgradeList.Clear();
        redUpgradeList.Add(new Upgrade (redUpgradeList.Count, 0, "More money", "Receive 100G", redUpgradeSprite[0], new Upgrade_Money()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 1, "Scavenger", "Doubles the amount of ammo gain after possessing.", redUpgradeSprite[redUpgradeList.Count], new Upgrade_WeaponAmmo()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 2, "Unstable system", "Shock wave now deals damage equal to your card damage", redUpgradeSprite[redUpgradeList.Count], new Upgrade_ShockWaveDamage()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 3, "Buggy card", "Card will create shockwave on impact", redUpgradeSprite[redUpgradeList.Count], new Upgrade_CardShockWave()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 1, "Double-edge", "+20% damage deal, +20% damage taken", redUpgradeSprite[redUpgradeList.Count], new Upgrade_DoubleEdge()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 3, "Rage", "+2% atk damage everytime you take damage. (Reset on new room).", redUpgradeSprite[redUpgradeList.Count], new Upgrade_Rage()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 1, "Close combat", "Enemy takes +30% damage when fighting close range.", redUpgradeSprite[redUpgradeList.Count], new Upgrade_CloseCombat()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 1, "Sharp shooter", "Enemy takes +30% damage when fighting long range.", redUpgradeSprite[redUpgradeList.Count], new Upgrade_SharpShooter()));
        redUpgradeList.Add(new Upgrade(redUpgradeList.Count, 2, "Stubborn", "Gains +2% Atk damage for every 1% missing health.", redUpgradeSprite[redUpgradeList.Count], new Upgrade_Stubborn()));

        greenUpgradeList.Clear();
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 0, "More money", "Receive 100G", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_Money()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 1, "Long Shock", "Increase stun duration of shockwave when possess.", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_ShockWaveStun()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 3, "Strong Feet", "+1 more dash.", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_ExtraDash()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 2, "Fleet Footwork", "Increase movement speed.", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_MovementSpeed()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 1, "Quick shot", "Increase atk speed.", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_ShockWaveStun()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 1, "Swift movement", "Gains a temporary speed boost after dashing.", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_SwiftMovement()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 3, "Feather Step", "No longer triggers trap.", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_FeatherStep()));
        greenUpgradeList.Add(new Upgrade(greenUpgradeList.Count, 2, "Shadow Movement", "Increase movement speed greatly after possessing", greenUpgradeSprite[greenUpgradeList.Count], new Upgrade_ShadowMovement()));

        blueUpgradeList.Clear();
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 0, "More money", "Receive 100G", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_Money()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 1, "Big Shock", "Increase size of shockwave when possess.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_ShockWaveRange()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 1, "Strong stomach", "Doubles the amount of healing after possessing.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_PHealing()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 1, "Moving fort", "Gains a temporary def boost after dashing.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_MovingFort()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 2, "Poison Resistance", "Recover from poison faster.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_PoisonResistance()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 2, "Fire Resistance", "Recover from burn faster.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_FireResistance()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 3, "Shock Armor", "Create a shockwave every 5 hits taken.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_ShockArmor()));
        blueUpgradeList.Add(new Upgrade(blueUpgradeList.Count, 3, "Shock Blast", "Shock wave now pushs all enemies away.", blueUpgradeSprite[blueUpgradeList.Count], new Upgrade_ShockBlast()));

        bulletModList.Clear();
        bulletModList.Add(new Upgrade(bulletModList.Count, 0, "More money", "Receive 100G", bulletModSprite[0], new Upgrade_Money()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Pyromancer", "Gun weapon has 15% to add a fire stack. Gun weapon x3 ammo.", bulletModSprite[1], new Upgrade_BurnBullet()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Good cable", "Charge weapon charges faster by 25%. Charge weapon x3 ammo.", bulletModSprite[2], new Upgrade_FasterCharge()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Unseen blade", "Melee weapon deals 150% damage when attack behind. Melee weapon x3 ammo.", bulletModSprite[3], new Upgrade_UnseenBlade()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "BEEG", "Increase projectile weapon size by 50%. Projectile weapon x3 ammo.", bulletModSprite[4], new Upgrade_BEEGBomb()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Sharpshooter", "Gun weapon has 10% of dealing triple damage. Gun weapon x3 ammo.", bulletModSprite[5], new Upgrade_CritBullet()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Good reflex", "Melee weapon now reflect enemey bullet. Melee weapon x3 ammo.", bulletModSprite[6], new Upgrade_ReflectSword()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Sharp bullet", "Gun weapon has 50% to pierce enemy. Gun weapon x3 ammo.", bulletModSprite[7], new Upgrade_SharpBullet()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Wild card", "No longer get weapon from possess. Get random weapons every 10 seconds.", bulletModSprite[8], new Upgrade_WildCard()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Engineer", "Summons have double HP. Special weapon x3 ammo.", bulletModSprite[9], new Upgrade_SturdyBuild()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Golden Touch", "Summons drops 2G on destroy. Special weapon x3 ammo.", bulletModSprite[10], new Upgrade_GoldBuild()));


        // slowly but surely: Slower charge but Stronger effect + damage when charge
        // Big hand: Reduce speed penalty for charge weapon.
        // Swift dance: Gain small speed boost after using melee weapon.
        // FEARS: Projectiles apply DEF DOWN for 5 seconds (does not stack);
    }
}

public class Upgrade
{
    public int id;
    public int tier;
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public UpgradeBaseEffect upgradeBaseEffect;

    public Upgrade(int Id, int Tier, string UpgradeName, string UpgradeDescription, Sprite UpgradeSprite, UpgradeBaseEffect UpgradeBaseEffect)
    {
        id = Id;
        tier = Tier;
        upgradeName = UpgradeName;
        upgradeDescription = UpgradeDescription;
        upgradeSprite = UpgradeSprite;
        upgradeBaseEffect = UpgradeBaseEffect;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour
{
    public static List<Upgrade> levelUpgradeList = new List<Upgrade>();
    public Sprite[] levelUpUpgradeSprite;
    public static List<Upgrade> bulletModList = new List<Upgrade>();
    public Sprite[] bulletModSprite;
    private void Awake()
    {
        levelUpgradeList.Clear();
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 0, "More money", "Receive 100G", levelUpUpgradeSprite[0], new Upgrade_Money()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Better HP", "Increase max HP by 50", levelUpUpgradeSprite[1], new Upgrade_HP1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Greater HP", "Increase max HP by 75", levelUpUpgradeSprite[2], new Upgrade_HP2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Greatest HP", "Increase max HP by 125", levelUpUpgradeSprite[3], new Upgrade_HP3()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Big Shock", "Increase size of shockwave slightly when possess.", levelUpUpgradeSprite[4], new Upgrade_ShockWaveRange1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Bigger Shock", "Increase size of shockwave moderately when possess.", levelUpUpgradeSprite[5], new Upgrade_ShockWaveRange2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Biggest Shock", "Increase size of shockwave greatly when possess.", levelUpUpgradeSprite[6], new Upgrade_ShockWaveRange3()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Long Shock", "Increase stun duration of shockwave slightly when possess.", levelUpUpgradeSprite[7], new Upgrade_ShockWaveStun1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Longer Shock", "Increase stun duration of shockwave moderately when possess.", levelUpUpgradeSprite[8], new Upgrade_ShockWaveStun2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Longest Shock", "Increase stun duration of shockwave greatly when possess.", levelUpUpgradeSprite[9], new Upgrade_ShockWaveStun3()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Strong Feet", "+1 more dash.", levelUpUpgradeSprite[10], new Upgrade_ExtraDash()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Fleet Footwork", "Increase movement speed.", levelUpUpgradeSprite[11], new Upgrade_MovementSpeed()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Sharp card", "Increase card damage by 10.", levelUpUpgradeSprite[12], new Upgrade_CardDamage1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Sharper card", "Increase card damage by 20.", levelUpUpgradeSprite[13], new Upgrade_CardDamage2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Sharpest card", "Increase card damage by 40.", levelUpUpgradeSprite[14], new Upgrade_CardDamage3()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Scavenger", "Increase ammo gain after possessing by 25%.", levelUpUpgradeSprite[15], new Upgrade_WeaponAmmo1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Looter", "Increase ammo gain after possessing by 75%.", levelUpUpgradeSprite[16], new Upgrade_WeaponAmmo2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Hoarder", "Increase ammo gain after possessing by 150%.", levelUpUpgradeSprite[17], new Upgrade_WeaponAmmo3()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Strong stomach", "Increase healing after possessing by 20%.", levelUpUpgradeSprite[18], new Upgrade_PHealing1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Stronger stomach", "Increase healing after possessing by 50%.", levelUpUpgradeSprite[19], new Upgrade_PHealing2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Strongest stomach", "Increase healing after possessing by 80%.", levelUpUpgradeSprite[20], new Upgrade_PHealing3()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Unstable system", "Shock wave now deals damage equal to your card damage", levelUpUpgradeSprite[21], new Upgrade_ShockWaveDamage()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Buggy card", "Card will create shockwave on impact", levelUpUpgradeSprite[22], new Upgrade_CardShockWave()));

        bulletModList.Clear();
        bulletModList.Add(new Upgrade(bulletModList.Count, 0, "More money", "Receive 100G", bulletModSprite[0], new Upgrade_Money()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Pyromancer", "Gun weapon has 10% to add a fire stack. Gun weapon x3 ammo.", bulletModSprite[1], new Upgrade_BurnBullet()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Good cable", "Charge weapon charges faster by 25%. Charge weapon x3 ammo.", bulletModSprite[2], new Upgrade_FasterCharge()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Unseen blade", "Melee weapon deals 150% damage when attack behind. Melee weapon x3 ammo.", bulletModSprite[3], new Upgrade_UnseenBlade()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "BEEG", "Increase projectile weapon size by 50%. Projectile weapon x3 ammo.", bulletModSprite[4], new Upgrade_BEEGBomb()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Sharpshooter", "Gun weapon has 10% of dealing triple damage. Gun weapon x3 ammo.", bulletModSprite[5], new Upgrade_CritBullet()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Good reflex", "Melee weapon now reflect enemey bullet. Melee weapon x3 ammo.", bulletModSprite[6], new Upgrade_ReflectSword()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Sharp bullet", "Gun weapon has 50% to pierce enemy. Gun weapon x3 ammo.", bulletModSprite[7], new Upgrade_SharpBullet()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Wild card", "No longer get weapon from possess. Get random weapons every 10 seconds.", bulletModSprite[8], new Upgrade_WildCard()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Engineer", "Summons have double HP. Special weapon x3 ammo.", bulletModSprite[9], new Upgrade_SturdyBuild()));
        bulletModList.Add(new Upgrade(bulletModList.Count, 1, "Golden Touch", "Summons drops 2G on destroy. Special weapon x3 ammo.", bulletModSprite[10], new Upgrade_GoldBuild()));

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour
{
    public static List<Upgrade> levelUpgradeList = new List<Upgrade>();
    public Sprite[] levelUpUpgradeSprite;
    public static List<Upgrade> stageUpgradeList = new List<Upgrade>();
    public Sprite[] stageUpgradeSprite;
    private void Awake()
    {
        levelUpgradeList.Clear();
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 0, "More money", "Receive 100P", levelUpUpgradeSprite[0], new Upgrade_Money()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 1, "Better HP", "Increase max HP by 25", levelUpUpgradeSprite[1], new Upgrade_HP1()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 2, "Greater HP", "Increase max HP by 50", levelUpUpgradeSprite[2], new Upgrade_HP2()));
        levelUpgradeList.Add(new Upgrade(levelUpgradeList.Count, 3, "Greatest HP", "Increase max HP by 100", levelUpUpgradeSprite[3], new Upgrade_HP3()));
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

        stageUpgradeList.Clear();
        stageUpgradeList.Add(new Upgrade(stageUpgradeList.Count, 0, "More money", "Receive 100P", levelUpUpgradeSprite[0], new Upgrade_Money()));
        stageUpgradeList.Add(new Upgrade(stageUpgradeList.Count, 2, "Pyromancer", "All weapon has 15% to burn enemy.", levelUpUpgradeSprite[0], null));
        stageUpgradeList.Add(new Upgrade(stageUpgradeList.Count, 1, "Good cable", "Charge weapon charges faster by 25%.", levelUpUpgradeSprite[0], null));
        stageUpgradeList.Add(new Upgrade(stageUpgradeList.Count, 3, "Blacksmith", "Melee weapon has infinite ammo.", levelUpUpgradeSprite[0], null));
        stageUpgradeList.Add(new Upgrade(stageUpgradeList.Count, 1, "BEEG", "Increase weapon size by 25%.", levelUpUpgradeSprite[0], null));
        stageUpgradeList.Add(new Upgrade(stageUpgradeList.Count, 1, "Sharpshooter", "Weapon has 10% crit rate (dealing triple damage).", levelUpUpgradeSprite[0], null));
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
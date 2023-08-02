using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour
{
    public static List<Upgrade> upgradeList = new List<Upgrade>();
    public Sprite[] upgradeSprite;
    private void Awake()
    {
        upgradeList.Clear();
        upgradeList.Add(new Upgrade(upgradeList.Count, 0, "More money", "Receive 100P", upgradeSprite[0], new Upgrade_Money()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Better HP", "Increase max HP by 25", upgradeSprite[1], new Upgrade_HP1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Greater HP", "Increase max HP by 50", upgradeSprite[2], new Upgrade_HP2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Greatest HP", "Increase max HP by 100", upgradeSprite[3], new Upgrade_HP3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Big Shock", "Increase size of shockwave slightly when possess.", upgradeSprite[4], new Upgrade_ShockWaveRange1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Bigger Shock", "Increase size of shockwave moderately when possess.", upgradeSprite[5], new Upgrade_ShockWaveRange2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Biggest Shock", "Increase size of shockwave greatly when possess.", upgradeSprite[6], new Upgrade_ShockWaveRange3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Long Shock", "Increase stun duration of shockwave slightly when possess.", upgradeSprite[7], new Upgrade_ShockWaveStun1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Longer Shock", "Increase stun duration of shockwave moderately when possess.", upgradeSprite[8], new Upgrade_ShockWaveStun2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Longest Shock", "Increase stun duration of shockwave greatly when possess.", upgradeSprite[9], new Upgrade_ShockWaveStun3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Strong Feet", "+1 more dash.", upgradeSprite[10], new Upgrade_ExtraDash()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Fleet Footwork", "Increase movement speed.", upgradeSprite[11], new Upgrade_MovementSpeed()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Sharp card", "Increase card damage by 10.", upgradeSprite[12], new Upgrade_CardDamage1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Sharper card", "Increase card damage by 20.", upgradeSprite[13], new Upgrade_CardDamage2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Sharpest card", "Increase card damage by 40.", upgradeSprite[14], new Upgrade_CardDamage3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Scavenger", "Increase ammo gain after possessing by 25%.", upgradeSprite[15], new Upgrade_WeaponAmmo1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Looter", "Increase ammo gain after possessing by 75%.", upgradeSprite[16], new Upgrade_WeaponAmmo2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Hoarder", "Increase ammo gain after possessing by 150%.", upgradeSprite[17], new Upgrade_WeaponAmmo3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Strong stomach", "Increase healing after possessing by 20%.", upgradeSprite[18], new Upgrade_PHealing1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Stronger stomach", "Increase healing after possessing by 50%.", upgradeSprite[19], new Upgrade_PHealing2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Strongest stomach", "Increase healing after possessing by 80%.", upgradeSprite[20], new Upgrade_PHealing3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Unstable system", "Shock wave now deals damage equal to your card damage", upgradeSprite[21], new Upgrade_ShockWaveDamage()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Buggy card", "Card will create shockwave on impact", upgradeSprite[22], null));
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
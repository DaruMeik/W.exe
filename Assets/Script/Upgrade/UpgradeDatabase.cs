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
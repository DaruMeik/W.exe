using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour
{
    public static List<Upgrade> upgradeList = new List<Upgrade>();
    private void Awake()
    {
        upgradeList.Clear();
        upgradeList.Add(new Upgrade(upgradeList.Count, 0, "More money", "Receive 100P", new Upgrade_Money()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Better HP", "Increase max HP by 25", new Upgrade_HP1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Greater HP", "Increase max HP by 50", new Upgrade_HP2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Greatest HP", "Increase max HP by 100", new Upgrade_HP3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Big Shock", "Increase size of shockwave slightly when possess.", new Upgrade_ShockWaveRange1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Bigger Shock", "Increase size of shockwave moderately when possess.", new Upgrade_ShockWaveRange2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Biggest Shock", "Increase size of shockwave greatly when possess.", new Upgrade_ShockWaveRange3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 1, "Long Shock", "Increase stunt duration of shockwave slightly when possess.", new Upgrade_ShockWaveStunt1()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 2, "Longer Shock", "Increase stunt duration of shockwave moderately when possess.", new Upgrade_ShockWaveStunt2()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Longest Shock", "Increase stunt duration of shockwave greatly when possess.", new Upgrade_ShockWaveStunt3()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Strong Feet", "+1 more dash.", new Upgrade_ExtraDash()));
        upgradeList.Add(new Upgrade(upgradeList.Count, 3, "Fleet Footwork", "Increase movement speed.", new Upgrade_MovementSpeed()));
    }
}

public class Upgrade
{
    public int id;
    public int tier;
    public string upgradeName;
    public string upgradeDescription;
    public UpgradeBaseEffect upgradeBaseEffect;

    public Upgrade(int Id, int Tier, string UpgradeName, string UpgradeDescription, UpgradeBaseEffect UpgradeBaseEffect)
    {
        id = Id;
        tier = Tier;
        upgradeName = UpgradeName;
        upgradeDescription = UpgradeDescription;
        upgradeBaseEffect = UpgradeBaseEffect;
    }
}
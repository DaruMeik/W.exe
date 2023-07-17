using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    public static List<Weapon> weaponList = new List<Weapon>();
    public static Weapon fishingMail;

    public GameObject[] weaponHitBox;
    public Sprite[] weaponSprite;

    public GameObject fishingMailObj;

    private void OnEnable()
    {
        weaponList.Add(new Weapon(0, "Buster", "00_Buster", "Semiauto", -1, 1, 20, 10, 90, 0, "Your trustworthy pistol", weaponHitBox[0], weaponSprite[0], new Wea00_BusterEffect()));
        weaponList.Add(new Weapon(1, "GatlingGun", null, "Fullauto", 50, 1, 12, 50, 80, 30, "Simple Fullauto gun", weaponHitBox[1], weaponSprite[1], new Wea01_GatlingEffect()));
        weaponList.Add(new Weapon(2, "ShortSword", "02_ShortSword", "Melee", -1, 1, 50, 12, 100, 0, "Simple short sword", weaponHitBox[2], weaponSprite[2], new Wea02_ShortSwordEffect()));
        fishingMail = new Weapon(-1, "Fishing Mail", "Fishing", "Special", -1, 1, 50, 20, 100, 0, "Send a mail", fishingMailObj, null, new FishingMailEffect());
    }
}

public class Weapon
{
    public int id;
    public string weaponName;
    public string triggerId;
    public string weaponType; 
    public int maxAmmo;
    public int burstAmmount;
    public int power;
    public int atkSpd;
    public int accuracy;
    public int speedPenalty;
    public string weaponDescription;
    public GameObject weaponHitBox;
    public Sprite weaponSprite;
    public WeaponBaseEffect weaponBaseEffect;

    public Weapon(int Id, string CardName, string TriggerId, string WeaponType, int MaxAmmo, int BurstAmmount, int Power, int AtkSpd, int Accuracy, int SpeedPenalty, string WeaponDescription, GameObject WeaponHitBox, Sprite WeaponSprite, WeaponBaseEffect WeaponBaseEffect)
    {
        id = Id;
        weaponName = CardName;
        triggerId = TriggerId;
        weaponType = WeaponType;
        maxAmmo = MaxAmmo;
        burstAmmount = BurstAmmount;
        power = Power;
        atkSpd = AtkSpd;
        accuracy = Accuracy;
        speedPenalty = SpeedPenalty;
        weaponHitBox = WeaponHitBox;
        weaponDescription = WeaponDescription;
        weaponSprite = WeaponSprite;
        weaponBaseEffect = WeaponBaseEffect;
    }
}
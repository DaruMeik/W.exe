using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    public static List<Weapon> weaponList = new List<Weapon>();
    public static Weapon fishingMail;
    public static Weapon tutorialSpecialBullet;

    public GameObject[] weaponHitBox;
    public Sprite[] weaponSprite;

    public GameObject fishingMailObj;
    public GameObject tutorialSpecialBulletObj;

    private void OnEnable()
    {
        weaponList.Add(new Weapon(0, "Buster", "00_Buster", "Semiauto", -1, 18, 10, 90, 0, "Your trustworthy pistol", 0, weaponHitBox[0], weaponSprite[0], new Wea00_BusterEffect()));
        weaponList.Add(new Weapon(1, "GatlingGun", null, "Fullauto", 15, 8, 50, 80, 30, "Simple Fullauto gun", 50, weaponHitBox[1], weaponSprite[1], new Wea01_GatlingEffect()));
        weaponList.Add(new Weapon(2, "ShortSword", "02_ShortSword", "Melee", -1, 25, 12, 100, 0, "Simple short sword", 50, weaponHitBox[2], weaponSprite[2], new Wea02_ShortSwordEffect()));
        weaponList.Add(new Weapon(3, "Shotgun", "03_Shotgun", "Semiauto", 6, 12, 5, 100, 0, "Simple shotgun", 50, weaponHitBox[3], weaponSprite[3], new Wea03_ShotgunEffect()));
        weaponList.Add(new Weapon(4, "SmallGrenade", null, "Semiauto", 3, 40, 5, 100, 0, "A small grenade", 50, weaponHitBox[4], weaponSprite[4], new Wea04_SmallGrenadeEffect()));
        weaponList.Add(new Weapon(5, "ZapCanon", null, "Charge", 5, 10, 50, 100, 50, "The longer you charge, the better", 50, weaponHitBox[5], weaponSprite[5], new Wea05_ZapCanonEffect()));

        fishingMail = new Weapon(-1, "Fishing Mail", "Fishing", "Special", -1, 30, 20, 100, 0, "Send a mail", 0, fishingMailObj, null, new FishingMailEffect());
        tutorialSpecialBullet = new Weapon(-2, "Buster", "00_Buster", "Semiauto", -1, 100, 10, 90, 0, "Your trustworthy pistol", 0, tutorialSpecialBulletObj, weaponSprite[0], new SpecialBulletEffect());
    }
}

public class Weapon
{
    public int id;
    public string weaponName;
    public string triggerId;
    public string weaponType;
    public int maxAmmo;
    public int power;
    public int atkSpd;
    public int accuracy;
    public int speedPenalty;
    public string weaponDescription;
    public int price;
    public GameObject weaponHitBox;
    public Sprite weaponSprite;
    public WeaponBaseEffect weaponBaseEffect;

    public Weapon(int Id, string CardName, string TriggerId, string WeaponType, int MaxAmmo, int Power, int AtkSpd, int Accuracy, int SpeedPenalty, string WeaponDescription, int Price, GameObject WeaponHitBox, Sprite WeaponSprite, WeaponBaseEffect WeaponBaseEffect)
    {
        id = Id;
        weaponName = CardName;
        triggerId = TriggerId;
        weaponType = WeaponType;
        maxAmmo = MaxAmmo;
        power = Power;
        atkSpd = AtkSpd;
        accuracy = Accuracy;
        speedPenalty = SpeedPenalty;
        weaponHitBox = WeaponHitBox;
        weaponDescription = WeaponDescription;
        price = Price;
        weaponSprite = WeaponSprite;
        weaponBaseEffect = WeaponBaseEffect;
    }
}
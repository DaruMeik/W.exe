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
        weaponList.Add(new Weapon(0, "Buster", "00_Buster", "Gun", -1, 8, 20, 75, 0, "Your trustworthy pistol", 0, weaponHitBox[0], weaponSprite[0], new Wea00_BusterEffect()));
        weaponList.Add(new Weapon(1, "GatlingGun", null, "Gun", 20, 6, 50, 75, 30, "Simple Fullauto gun", 1, weaponHitBox[1], weaponSprite[1], new Wea01_GatlingEffect()));
        weaponList.Add(new Weapon(2, "ShortSword", "02_ShortSword", "Melee", 10, 20, 12, 100, 0, "Simple short sword", 1, weaponHitBox[2], weaponSprite[2], new Wea02_ShortSwordEffect()));
        weaponList.Add(new Weapon(3, "Shotgun", "03_Shotgun", "Gun", 6, 10, 5, 100, 0, "Simple shotgun", 1, weaponHitBox[3], weaponSprite[3], new Wea03_ShotgunEffect()));
        weaponList.Add(new Weapon(4, "SmallGrenade", null, "Projectile", 3, 35, 5, 100, 0, "A small grenade", 1, weaponHitBox[4], weaponSprite[4], new Wea04_SmallGrenadeEffect()));
        weaponList.Add(new Weapon(5, "ZapCanon", null, "Charge", 3, 8, 50, 100, 50, "The longer you charge, the better", 1, weaponHitBox[5], weaponSprite[5], new Wea05_ZapCanonEffect()));
        weaponList.Add(new Weapon(6, "KnockFist", "06_KnockFist", "Melee", -1, 25, 4, 100, 0, "Punch enemy into oblivion", 0, weaponHitBox[6], weaponSprite[6], new Wea06_KnockFistEffect()));
        weaponList.Add(new Weapon(7, "Timer", null, "Special", 5, 20, 10, 100, 0, "Set up a slowly-exploded seed.", 1, weaponHitBox[7], weaponSprite[7], new Wea07_TimerSeedEffect()));
        weaponList.Add(new Weapon(8, "ProtectorLantern", "08_ProtectorLantern", "Special", 1, 60, 5, 100, 0, "Give you a shield in one direction (can't stack)", 1, weaponHitBox[8], weaponSprite[8], new Wea08_ProtectorLanternEffect()));
        weaponList.Add(new Weapon(9, "PaintGun", null, "Gun", -1, 1, 30, 80, 30, "Shoot mud that slow targets", 0, weaponHitBox[9], weaponSprite[9], new Wea09_PaintGunEffect()));
        weaponList.Add(new Weapon(10, "PoisonGrenade", null, "Projectile", 3, 15, 3, 100, 0, "Create a small poison pool", 1, weaponHitBox[10], weaponSprite[10], new Wea10_PoisonGrenadeEffect()));
        weaponList.Add(new Weapon(11, "ChargeSpear", "11_ChargeSpear", "Melee", 2, 30, 5, 100, 0, "Charge ahead", 1, weaponHitBox[11], weaponSprite[11], new Wea11_ChargeSpearEffect()));

        fishingMail = new Weapon(-1, "Fishing Mail", "Fishing", "Special", -1, 25, 20, 100, 0, "Send a mail", 0, fishingMailObj, null, new FishingMailEffect());
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
    public int tier;
    public GameObject weaponHitBox;
    public Sprite weaponSprite;
    public WeaponBaseEffect weaponBaseEffect;

    public Weapon(int Id, string CardName, string TriggerId, string WeaponType, int MaxAmmo, int Power, int AtkSpd, int Accuracy, int SpeedPenalty, string WeaponDescription, int Tier, GameObject WeaponHitBox, Sprite WeaponSprite, WeaponBaseEffect WeaponBaseEffect)
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
        tier = Tier;
        weaponSprite = WeaponSprite;
        weaponBaseEffect = WeaponBaseEffect;
    }
}
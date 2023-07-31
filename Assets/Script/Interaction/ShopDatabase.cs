using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDatabase : MonoBehaviour
{
    public static List<ShopItem> weaponList = new List<ShopItem>();
    public static List<ShopItem> upgradeList = new List<ShopItem>();

    private void OnEnable()
    {
        for (int i = 1; i < WeaponDatabase.weaponList.Count; i++)
        {
            weaponList.Add(new ShopItem(i, WeaponDatabase.weaponList[i].weaponName, WeaponDatabase.weaponList[i].weaponDescription, WeaponDatabase.weaponList[i].price, WeaponDatabase.weaponList[i].weaponSprite));
        }
        for (int i = 0; i < UpgradeDatabase.upgradeList.Count; i++)
        {
            upgradeList.Add(new ShopItem(i, UpgradeDatabase.upgradeList[i].upgradeName, UpgradeDatabase.upgradeList[i].upgradeDescription, UpgradeDatabase.upgradeList[i].tier * 50, UpgradeDatabase.upgradeList[i].upgradeSprite));
        }
    }
}

public class ShopItem
{
    public int id;
    public string name;
    public string description;
    public int price;
    public Sprite sprite;

    public ShopItem(int Id, string Name, string Description, int Price, Sprite Sprite)
    {
        this.id = Id;
        this.name = Name;
        this.description = Description;
        this.price = Price;
        this.sprite = Sprite;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDatabase : MonoBehaviour
{
    public static List<ShopItem> weaponList = new List<ShopItem>();
    public static List<ShopItem> utilityList = new List<ShopItem>();
    public Sprite[] utilitySprites;
    public GameObject[] utilityGObjs;
    public static List<ShopItem> expList = new List<ShopItem>();
    public Sprite[] expSprites;
    public GameObject[] expGObjs;

    private void OnEnable()
    {
        for (int i = 0; i < WeaponDatabase.weaponList.Count; i++)
        {
            weaponList.Add(new ShopItem(i, WeaponDatabase.weaponList[i].weaponName, WeaponDatabase.weaponList[i].weaponDescription, 100, WeaponDatabase.weaponList[i].weaponSprite));
        }
        utilityList.Add(new ShopItem(utilityList.Count, "Med Kit", "Heals 50 HP", 75, utilitySprites[utilityList.Count], utilityGObjs[utilityList.Count]));
        utilityList.Add(new ShopItem(utilityList.Count, "Ammo Kit", "Add 1 full ammo stock to current weapons", 100, utilitySprites[utilityList.Count], utilityGObjs[utilityList.Count]));
        utilityList.Add(new ShopItem(utilityList.Count, "Upgrade Kit", "Increase your max health by 25", 125, utilitySprites[utilityList.Count], utilityGObjs[utilityList.Count]));

        expList.Add(new ShopItem(expList.Count, "Red Exp", "Gains 1 exp in offense", 200, expSprites[expList.Count], expGObjs[expList.Count]));
        expList.Add(new ShopItem(expList.Count, "Green Exp", "Gains 1 exp in mobility", 200, expSprites[expList.Count], expGObjs[expList.Count]));
        expList.Add(new ShopItem(expList.Count, "Blue Exp", "Gains 1 exp in defense", 200, expSprites[expList.Count], expGObjs[expList.Count]));
        expList.Add(new ShopItem(expList.Count, "Random Exp", "Gains 1 random exp", 150, expSprites[expList.Count], expGObjs[expList.Count]));
    }
}

public class ShopItem
{
    public int id;
    public string name;
    public string description;
    public int price;
    public Sprite sprite;
    public GameObject interactObject;

    public ShopItem(int Id, string Name, string Description, int Price, Sprite Sprite, GameObject InteractObject = null)
    {
        this.id = Id;
        this.name = Name;
        this.description = Description;
        this.price = Price;
        this.sprite = Sprite;
        this.interactObject = InteractObject;
    }
}
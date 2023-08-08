using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public BuyItem[] items;
    [SerializeField] private PlayerStat playerStat;
    private void OnEnable()
    {
        foreach (BuyItem item in items)
        {
            item.gameObject.SetActive(false);
        }

        List<int> possibleId = new List<int>();
        possibleId.Clear();
        foreach (ShopItem item in ShopDatabase.weaponList)
        {
            possibleId.Add(item.id);
        }
        //Roll for weapon
        items[0].itemType = "Weapon";
        items[0].itemId = Random.Range(0, possibleId.Count);
        possibleId.Remove(items[0].itemId);
        items[1].itemType = "Weapon";
        items[1].itemId = Random.Range(0, possibleId.Count);

        //Roll for upgrade
        possibleId = new List<int>();
        possibleId.Clear();
        foreach (ShopItem item in ShopDatabase.upgradeList)
        {
            possibleId.Add(item.id);
        }
        var temp = possibleId.Where(x => !playerStat.levelUpgradeRegister.Contains(x) && x != 0);
        items[2].itemType = "Upgrade";
        if (temp.Count() > 0)
            items[2].itemId = temp.ElementAt(Random.Range(0, temp.Count()));
        else
            items[2].itemId = 0;
        Debug.Log("Before");
        foreach(int ite in temp)
        {
            Debug.Log(ite);
        }
        temp = temp.Where(x => x != items[2].itemId);
        Debug.Log("After");
        foreach (int ite in temp)
        {
            Debug.Log(ite);
        }
        items[3].itemType = "Upgrade";
        if (temp.Count() > 0)
            items[3].itemId = temp.ElementAt(Random.Range(0, temp.Count()));
        else
            items[3].itemId = 0;


        foreach (BuyItem item in items)
        {
            item.gameObject.SetActive(true);
        }
    }
}

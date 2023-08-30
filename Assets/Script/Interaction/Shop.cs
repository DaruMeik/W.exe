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

        items[0].itemType = "Utility";
        items[0].itemId = ShopDatabase.utilityList[Random.Range(0, ShopDatabase.utilityList.Count)].id;

        List<int> possibleId = new List<int>();
        possibleId.Clear();
        foreach (ShopItem item in ShopDatabase.weaponList)
        {
            if(!playerStat.unsellableWeapon.Contains(item.id))
                possibleId.Add(item.id);
        }
        //Roll for weapon
        items[1].itemType = "Weapon";
        items[1].itemId = possibleId[Random.Range(0, possibleId.Count)];
        possibleId.Remove(items[1].itemId);
        items[2].itemType = "Weapon";
        items[2].itemId = possibleId[Random.Range(0, possibleId.Count)];
        possibleId.Remove(items[2].itemId);

        items[3].itemType = "Exp";
        items[3].itemId = ShopDatabase.expList[Random.Range(0, ShopDatabase.expList.Count)].id;

        foreach (BuyItem item in items)
        {
            item.gameObject.SetActive(true);
        }
    }
}

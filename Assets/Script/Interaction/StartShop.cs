using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShop : MonoBehaviour
{
    public BuyItem item;
    [SerializeField] private PlayerStat playerStat;
    private void OnEnable()
    {
        item.gameObject.SetActive(false);

        List<int> possibleId = new List<int>();
        possibleId.Clear();
        foreach (ShopItem item in ShopDatabase.weaponList)
        {
            if (!playerStat.unsellableWeapon.Contains(item.id))
                possibleId.Add(item.id);
        }
        //Roll for weapon
        item.itemType = "StarterWeapon";
        item.itemId = possibleId[Random.Range(0, possibleId.Count)];

        item.gameObject.SetActive(true);
    }
}

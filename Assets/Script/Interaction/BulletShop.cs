using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShop : MonoBehaviour
{
    public BulletMod[] items;
    [SerializeField] private PlayerStat playerStat;
    private void OnEnable()
    {
        foreach (BulletMod item in items)
        {
            item.gameObject.SetActive(false);
        }

        List<int> possibleId = new List<int>();
        possibleId.Clear();
        foreach (Upgrade item in UpgradeDatabase.bulletModList)
        {
            possibleId.Add(item.id);
        }
        var temp = possibleId.Where(x => !playerStat.bulletModRegister.Contains(x) && x != 0);
        if (temp.Count() > 0)
            items[0].itemId = temp.ElementAt(Random.Range(0, temp.Count()));
        else
            items[0].itemId = 0;

        temp = temp.Where(x => x != items[0].itemId);
        if (temp.Count() > 0)
            items[1].itemId = temp.ElementAt(Random.Range(0, temp.Count()));
        else
            items[1].itemId = 0;

        temp = temp.Where(x => x != items[1].itemId);
        if (temp.Count() > 0)
            items[2].itemId = temp.ElementAt(Random.Range(0, temp.Count()));
        else
            items[2].itemId = 0;


        foreach (BulletMod item in items)
        {
            item.gameObject.SetActive(true);
        }
    }
}

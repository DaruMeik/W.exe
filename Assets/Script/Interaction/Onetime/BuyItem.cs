using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : Onetime
{
    public int itemId;
    public string itemType;
    public int itemPrice;
    public WeaponSlotSelection weaponSlotSelection;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private TextAnimation textAnim;
    protected override void OnEnable()
    {
        base.OnEnable();
        textAnim.textBoxList.Clear();
        string temp = "";
        switch (itemType)
        {
            case "StarterWeapon":
                temp += ShopDatabase.weaponList[itemId].name + "\n" + ShopDatabase.weaponList[itemId].description;
                itemPrice = 0;
                spriteRenderer.sprite = ShopDatabase.weaponList[itemId].sprite;
                highlightRenderer.sprite = ShopDatabase.weaponList[itemId].sprite;
                break;
            case "Weapon":
                temp += ShopDatabase.weaponList[itemId].name + "\n" + ShopDatabase.weaponList[itemId].description + "\nPrice: " + ShopDatabase.weaponList[itemId].price + "P";
                itemPrice = ShopDatabase.weaponList[itemId].price;
                spriteRenderer.sprite = ShopDatabase.weaponList[itemId].sprite;
                highlightRenderer.sprite = ShopDatabase.weaponList[itemId].sprite;
                break;
            case "Upgrade":
                temp += ShopDatabase.upgradeList[itemId].name + "\n" + ShopDatabase.upgradeList[itemId].description + "\nPrice: " + ShopDatabase.upgradeList[itemId].price + "P";
                itemPrice = ShopDatabase.upgradeList[itemId].price;
                spriteRenderer.sprite = ShopDatabase.upgradeList[itemId].sprite;
                highlightRenderer.sprite = ShopDatabase.upgradeList[itemId].sprite;
                break;
        }
        textAnim.textBoxList.Add(temp);
    }
    public override void Interact()
    {
        if (playerStat.money < itemPrice)
            return;
        else
        {
            playerStat.money -= itemPrice;
            eventBroadcast.UpdateMoneyNoti();
            switch (itemType)
            {
                case "StarterWeapon":
                case "Weapon":
                    weaponSlotSelection.weaponID = ShopDatabase.weaponList[itemId].id;
                    weaponSlotSelection.gameObject.SetActive(true);
                    break;
                case "Upgrade":
                    UpgradeDatabase.levelUpgradeList[itemId].upgradeBaseEffect.ApplyEffect(playerStat);
                    playerStat.levelUpgradeRegister.Add(ShopDatabase.upgradeList[itemId].id);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

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
            case "Weapon":
                temp += ShopDatabase.weaponList[itemId].name + "\n" + ShopDatabase.weaponList[itemId].description + "\nPrice: " + ShopDatabase.weaponList[itemId].price + "P";
                itemPrice = ShopDatabase.weaponList[itemId].price;
                spriteRenderer.sprite = ShopDatabase.weaponList[itemId].sprite;
                highlightRenderer.sprite = ShopDatabase.weaponList[itemId].sprite;
                break;
            case "Utility":
                temp += ShopDatabase.utilityList[itemId].name + "\n" + ShopDatabase.utilityList[itemId].description + "\nPrice: " + ShopDatabase.utilityList[itemId].price + "P";
                itemPrice = ShopDatabase.utilityList[itemId].price;
                spriteRenderer.sprite = ShopDatabase.utilityList[itemId].sprite;
                highlightRenderer.sprite = ShopDatabase.utilityList[itemId].sprite;
                break;
            case "Exp":
                temp += ShopDatabase.expList[itemId].name + "\n" + ShopDatabase.expList[itemId].description + "\nPrice: " + ShopDatabase.expList[itemId].price + "P";
                itemPrice = ShopDatabase.expList[itemId].price;
                spriteRenderer.sprite = ShopDatabase.expList[itemId].sprite;
                highlightRenderer.sprite = ShopDatabase.expList[itemId].sprite;
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
                case "Utility":
                    Instantiate(ShopDatabase.utilityList[itemId].interactObject).GetComponent<Onetime>().Interact();
                    break;
                case "Exp":
                    Instantiate(ShopDatabase.expList[itemId].interactObject).GetComponent<Onetime>().Interact();
                    break;
            }
            Destroy(gameObject);
        }
    }
}

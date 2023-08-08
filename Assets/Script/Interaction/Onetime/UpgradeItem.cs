using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItem : Onetime
{
    public int itemId;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private TextAnimation textAnim;
    private void OnEnable()
    {
        TurnOffHighlight();
        textAnim.textBoxList.Clear();
        string temp = "";
        temp += UpgradeDatabase.stageUpgradeList[itemId].upgradeName + "\n" + UpgradeDatabase.stageUpgradeList[itemId].upgradeDescription;
        spriteRenderer.sprite = UpgradeDatabase.stageUpgradeList[itemId].upgradeSprite;
        highlightRenderer.sprite = UpgradeDatabase.stageUpgradeList[itemId].upgradeSprite;
        textAnim.textBoxList.Add(temp);
    }
    public override void Interact()
    {
        UpgradeDatabase.stageUpgradeList[itemId].upgradeBaseEffect.ApplyEffect(playerStat);
        playerStat.stageUpgradeRegister.Add(ShopDatabase.upgradeList[itemId].id);
        eventBroadcast.UpgradePickedNoti();
    }
}

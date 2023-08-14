using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMod : Onetime
{
    public int itemId;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private TextAnimation textAnim;
    protected override void OnEnable()
    {
        base.OnEnable();
        textAnim.textBoxList.Clear();
        string temp = "";
        temp += UpgradeDatabase.bulletModList[itemId].upgradeName + "\n" + UpgradeDatabase.bulletModList[itemId].upgradeDescription;
        spriteRenderer.sprite = UpgradeDatabase.bulletModList[itemId].upgradeSprite;
        highlightRenderer.sprite = UpgradeDatabase.bulletModList[itemId].upgradeSprite;
        textAnim.textBoxList.Add(temp);
        eventBroadcast.bulletModPicked += SelfDestruct;
    }
    private void OnDisable()
    {
        eventBroadcast.bulletModPicked -= SelfDestruct;
    }
    public override void Interact()
    {
        UpgradeDatabase.levelUpgradeList[itemId].upgradeBaseEffect.ApplyEffect(playerStat);
        playerStat.levelUpgradeRegister.Add(ShopDatabase.upgradeList[itemId].id);
        eventBroadcast.BulletModPickedNoti();
    }
    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
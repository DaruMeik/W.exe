using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public TextMeshProUGUI[] ammos;
    public GameObject[] weaponObjs;
    public Image[] images;
    public Image[] highlight;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.updateWeapon += UpdateInfo;
    }
    private void OnDisable()
    {
        eventBroadcast.updateWeapon -= UpdateInfo;
    }
    private void UpdateInfo()
    {
        ammos[0].text = (playerStat.currentAmmo[0] >= 0) ? playerStat.currentAmmo[0].ToString() : "Åá";
        ammos[1].text = (playerStat.currentAmmo[1] >= 0) ? playerStat.currentAmmo[1].ToString() : "Åá";
        switch (playerStat.currentIndex)
        {
            case 0:
                weaponObjs[0].transform.localScale = Vector3.one * 1.5f;
                weaponObjs[1].transform.localScale = Vector3.one;
                highlight[0].color = new Color32(116,240,133,255);
                highlight[1].color = new Color32(255, 255, 255, 255);
                break;
            case 1:
                weaponObjs[0].transform.localScale = Vector3.one;
                weaponObjs[1].transform.localScale = Vector3.one * 1.5f;
                highlight[0].color = new Color32(255, 255, 255, 255);
                highlight[1].color = new Color32(116, 240, 133, 255);
                break;
        }
        images[0].sprite = WeaponDatabase.weaponList[playerStat.currentWeapon[0]].weaponSprite;
        images[1].sprite = WeaponDatabase.weaponList[playerStat.currentWeapon[1]].weaponSprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotSelection : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;

    public int weaponID;
    private void OnEnable()
    {
        eventBroadcast.EnterUINoti();
        Time.timeScale = Mathf.Max(0, Time.timeScale - 1f);
    }
    private void OnDisable()
    {
        eventBroadcast.ExitUINoti();
        Time.timeScale = Mathf.Min(1f, Time.timeScale + 1f);
    }
    public void ChooseReward(int buttonIndex)
    {
        playerStat.currentWeapon[buttonIndex] = weaponID;
        playerStat.currentAmmo[buttonIndex] = WeaponDatabase.weaponList[weaponID].maxAmmo * 3;
        eventBroadcast.UpdateWeaponSpriteNoti();
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotSelection : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;

    public int weaponID;
    private float previousTimeScale;
    private void OnEnable()
    {
        eventBroadcast.EnterUINoti();
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        eventBroadcast.ExitUINoti();
        Time.timeScale = previousTimeScale;
    }
    public void ChooseReward(int buttonIndex)
    {
        playerStat.currentWeapon[buttonIndex] = weaponID;
        playerStat.currentAmmo[buttonIndex] = WeaponDatabase.weaponList[weaponID].maxAmmo * 3;
        eventBroadcast.UpdateWeaponSpriteNoti();
        gameObject.SetActive(false);
    }
}

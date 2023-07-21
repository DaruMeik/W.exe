using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Slider HPSlider;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.updateHP += UpdateHPBar;
    }
    private void OnDisable()
    {
        eventBroadcast.updateHP -= UpdateHPBar;
    }
    private void UpdateHPBar()
    {
        HPSlider.maxValue = playerStat.maxHP;
        HPSlider.value = playerStat.currentHP;
    }
}

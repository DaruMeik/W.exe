using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
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
        text.text = playerStat.currentHP.ToString() + " / " + playerStat.maxHP.ToString();
        HPSlider.maxValue = playerStat.maxHP;
        HPSlider.value = playerStat.currentHP;
    }
}

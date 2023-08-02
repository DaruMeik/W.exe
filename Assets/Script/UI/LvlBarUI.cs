using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LvlBarUI : MonoBehaviour
{
    [SerializeField] private Slider LvlSlider;
    [SerializeField] private TextMeshProUGUI LvlText;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.updateLvl += UpdateLvlBar;
    }
    private void OnDisable()
    {
        eventBroadcast.updateLvl -= UpdateLvlBar;
    }
    private void UpdateLvlBar()
    {
        LvlText.text = playerStat.level.ToString();
        LvlSlider.maxValue = playerStat.level;
        LvlSlider.value = playerStat.exp - (playerStat.level * (playerStat.level - 1)) / 2;
    }
}

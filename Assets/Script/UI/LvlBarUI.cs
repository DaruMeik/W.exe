using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LvlBarUI : MonoBehaviour
{
    [SerializeField] private Slider redLvlSlider;
    [SerializeField] private Slider greenLvlSlider;
    [SerializeField] private Slider blueLvlSlider;
    [SerializeField] private TextMeshProUGUI redExpText;
    [SerializeField] private TextMeshProUGUI greenExpText;
    [SerializeField] private TextMeshProUGUI blueExpText;
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
        redLvlSlider.maxValue = playerStat.redLevel;
        greenLvlSlider.maxValue = playerStat.greenLevel;
        blueLvlSlider.maxValue = playerStat.blueLevel;

        redLvlSlider.value = playerStat.redExp - (playerStat.redLevel * (playerStat.redLevel - 1)) / 2;
        greenLvlSlider.value = playerStat.greenExp - (playerStat.greenLevel * (playerStat.greenLevel - 1)) / 2;
        blueLvlSlider.value = playerStat.blueExp - (playerStat.blueLevel * (playerStat.blueLevel - 1)) / 2;

        redExpText.text = redLvlSlider.value.ToString() + " / " + redLvlSlider.maxValue.ToString();
        greenExpText.text = greenLvlSlider.value.ToString() + " / " + greenLvlSlider.maxValue.ToString();
        blueExpText.text = blueLvlSlider.value.ToString() + " / " + blueLvlSlider.maxValue.ToString();
    }
}

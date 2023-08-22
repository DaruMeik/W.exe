using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image cardImage;
    [SerializeField] private EventBroadcast eventBroadcast;
    [SerializeField] private PlayerStat playerStat;

    private void OnEnable()
    {
        eventBroadcast.updateCardUI += UpdateCard;
    }
    private void OnDisable()
    {
        eventBroadcast.updateCardUI -= UpdateCard;
    }
    private void UpdateCard()
    {
        slider.value = playerStat.cardReadyPerc;
        if(playerStat.cardReadyPerc == 100)
        {
            cardImage.enabled = true;
        }
        else
        {
            cardImage.enabled = false;
        }
    }
}

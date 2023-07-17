using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
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
        cardImage.enabled = playerStat.hasCard;
    }
}

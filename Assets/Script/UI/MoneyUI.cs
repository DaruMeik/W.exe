using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.updateMoney += UpdateMoneyText;
    }
    private void OnDisable()
    {
        eventBroadcast.updateMoney -= UpdateMoneyText;
    }
    private void UpdateMoneyText()
    {
        moneyText.text = playerStat.money.ToString();
    }
}

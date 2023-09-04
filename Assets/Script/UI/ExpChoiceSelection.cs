using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpChoiceSelection : MonoBehaviour
{
    public int amount;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.EnterUINoti();
        Time.timeScale = Mathf.Max(0, Time.timeScale - 1f);
        texts[0].text = "Gains " + amount + " exp in offense.";
        texts[1].text = "Gains " + amount + " exp in mobility.";
        texts[2].text = "Gains " + amount + " exp in defense.";
    }
    private void OnDisable()
    {
        eventBroadcast.ExitUINoti();
        Time.timeScale = Mathf.Min(1f, Time.timeScale + 1f);
    }
    public void ChooseReward(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                eventBroadcast.GainExpNoti(amount, "Red");
                break;
            case 1:
                eventBroadcast.GainExpNoti(amount, "Green");
                break;
            case 2:
                eventBroadcast.GainExpNoti(amount, "Blue");
                break;
        }
        gameObject.SetActive(false);
    }
}

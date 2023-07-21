using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public TextAnimation textAnim;
    public GameObject[] highlight;
    public GameObject textBox;
    public EventBroadcast eventBroadcast;
    public bool textIsOn;
    private void OnEnable()
    {
        TurnOffHighlight();
        eventBroadcast.finishText += FinishTextBox;
    }
    private void OnDisable()
    {
        eventBroadcast.finishText -= FinishTextBox;
    }
    public void TurnOnHighlight()
    {
        foreach (GameObject gObj in highlight)
        {
            gObj.SetActive(true);
        }
    }
    public void TurnOffHighlight()
    {
        foreach (GameObject gObj in highlight)
        {
            gObj.SetActive(false);
        }
        textBox.SetActive(false);
        textIsOn = false;
    }
    public void TurnOnText()
    {
        if (textIsOn)
        {
            return;
        }
        textIsOn = true;
        foreach (GameObject gObj in highlight)
        {
            gObj.SetActive(false);
        }
        textBox.SetActive(true);
    }

    private void FinishTextBox()
    {
        foreach (GameObject gObj in highlight)
        {
            gObj.SetActive(true);
        }
        textIsOn = false;
    }
}
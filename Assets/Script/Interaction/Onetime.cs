using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Onetime : MonoBehaviour
{
    public GameObject[] highlight;
    public bool hasBeenUsed = false;
    public PlayerStat playerStat;
    public EventBroadcast eventBroadcast;
    public Collider2D triggerCol;
    protected virtual void OnEnable()
    {
        hasBeenUsed = false;
        TurnOffHighlight();
    }

    public void TurnOnHighlight()
    {
        if (!hasBeenUsed)
        {
            foreach (GameObject gObj in highlight)
            {
                gObj.SetActive(true);
            }
        }
    }
    public void TurnOffHighlight()
    {
        foreach (GameObject gObj in highlight)
        {
            gObj.SetActive(false);
        }
    }

    public abstract void Interact();
}

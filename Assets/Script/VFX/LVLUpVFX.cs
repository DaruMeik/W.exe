using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLUpVFX : MonoBehaviour
{
    private float turnOffTime = 0f;
    private void OnEnable()
    {
        turnOffTime = Time.time + 1.5f;
    }
    private void Update()
    {
        if(Time.time > turnOffTime)
        {
            gameObject.SetActive(false);
        }
    }
}

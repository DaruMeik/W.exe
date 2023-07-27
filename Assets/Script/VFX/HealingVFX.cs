using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingVFX : MonoBehaviour
{
    private float destroyTime = 0;

    private void OnEnable()
    {
        destroyTime = Time.time + 0.75f;
    }
    private void Update()
    {
        if(Time.time > destroyTime)
        {
            gameObject.SetActive(false);
        }
    }
}

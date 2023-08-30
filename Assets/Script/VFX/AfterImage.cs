using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float fadeoutSpeed = 1f;
    public Animator animator;
    private void OnEnable()
    {
        animator.speed = fadeoutSpeed;
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}

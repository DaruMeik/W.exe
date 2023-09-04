using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.allDead += TurnOff;
    }
    private void OnDisable()
    {
        eventBroadcast.allDead -= TurnOff;
    }
    private void TurnOff()
    {
        animator.SetTrigger("TurnOff");
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}

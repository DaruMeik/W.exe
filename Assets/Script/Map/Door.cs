using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private EventBroadcast eventBroadcast;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        eventBroadcast.allDead += OpenDoor;
    }
    private void OnDisable()
    {
        eventBroadcast.allDead -= OpenDoor;
    }
    private void OpenDoor()
    {
        animator.SetTrigger("Open");
    }
}

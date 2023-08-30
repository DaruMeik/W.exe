using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossCameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera bossCam;
    public CinemachineVirtualCamera playerCam;
    [SerializeField] private EventBroadcast eventBroadcast;

    private void OnEnable()
    {
        Time.timeScale = 0.5f;
        playerCam.enabled = false;
        bossCam.enabled = true;

        eventBroadcast.bossSpawn += ReturnToPlayer;
    }

    private void ReturnToPlayer()
    {
        bossCam.enabled = false;
        playerCam.enabled = true;
        Time.timeScale = 1f;
    }
}

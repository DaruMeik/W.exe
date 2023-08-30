using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineCamControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cineCam;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    [SerializeField] private EventBroadcast eventBroadcast;
    private float a, b, startTime;
    private void OnEnable()
    {
        cinemachineBasicMultiChannelPerlin = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        eventBroadcast.cameraShake += Shake;
    }
    private void OnDisable()
    {
        eventBroadcast.cameraShake -= Shake;
    }
    private void Update()
    {
        if (startTime + b > Time.time)
        {
            float intensity = a * (Time.time - startTime) * (Time.time - startTime - b);
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
        else if (cinemachineBasicMultiChannelPerlin.m_AmplitudeGain != 0)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        }
    }

    private void Shake(float duration, float intensity)
    {
        b = duration;
        a = -4*intensity/Mathf.Pow(b,2);
        startTime = Time.time;
    }
}

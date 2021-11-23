using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CinemachineVirtualCamera virtualCamera;
    private static CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel;
    private static float Speed;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public static void cameraShake(float Amplitude,float duration,float Frequncy = 1f)
    {       
        cinemachineBasicMultiChannel.m_AmplitudeGain = Amplitude;
        Speed = Amplitude/duration;
        cinemachineBasicMultiChannel.m_FrequencyGain = Frequncy;
    }

    private void Update()
    {
        cinemachineBasicMultiChannel.m_AmplitudeGain = Mathf.Lerp(cinemachineBasicMultiChannel.m_AmplitudeGain, 0f, Time.deltaTime*Speed);       
    }
}

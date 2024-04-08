using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    [SerializeField] private float amplitude = 3f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private NoiseSettings noiseType;

    private void OnEnable()
    {
        BaseLineScript.OnGameFailed += BaseLineScript_OnLevelFailed;
    }

    private void BaseLineScript_OnLevelFailed()
    {
        var vCam = GetComponent<CinemachineBrain>();
        virtualCamera = (CinemachineVirtualCamera)vCam.ActiveVirtualCamera;

        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if(noise == null)
            {
                noise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                noise.m_NoiseProfile = noiseType;
            }
            //Debug.Log("VirtualCameraPresent " + virtualCamera.name);
        }
    }

    private void Start()
    {

    }

    public void ShakeCamera(float shakeDuration)
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;

            //Debug.Log("NoisePresent");
            StartCoroutine(StopShake(shakeDuration));
        }
        else
        {
            Debug.LogWarning("Noise is Null");
        }
    }

    private IEnumerator StopShake(float shakeDuration)
    {
        yield return new WaitForSeconds(shakeDuration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}

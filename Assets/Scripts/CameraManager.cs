using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance;
    public CinemachineVirtualCamera vmCam;

    private CinemachineBasicMultiChannelPerlin noiseSettings;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        noiseSettings = vmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitutde, float frequency, float timeInSeconds)
    {
        noiseSettings.m_AmplitudeGain = amplitutde;
        noiseSettings.m_FrequencyGain = frequency;
        StartCoroutine(StartScreenShake(timeInSeconds));
    }

    IEnumerator StartScreenShake(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        noiseSettings.m_AmplitudeGain = 0;
        noiseSettings.m_FrequencyGain = 0;
    }


}

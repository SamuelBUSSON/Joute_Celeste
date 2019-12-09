using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    public void Vibrate(float low_frequency, float high_frequency, float timeInSeconds, int playerIndex)
    {
        print(playerIndex);
        Gamepad.all[playerIndex].SetMotorSpeeds(low_frequency, high_frequency);
        StartCoroutine(StartVibrateController(timeInSeconds, playerIndex));
    }

    IEnumerator StartScreenShake(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        noiseSettings.m_AmplitudeGain = 0;
        noiseSettings.m_FrequencyGain = 0;
    }

    IEnumerator StartVibrateController(float timeInSeconds, int playerIndex)
    {
        yield return new WaitForSeconds(timeInSeconds);
        Gamepad.all[playerIndex].SetMotorSpeeds(0, 0);
    }
}

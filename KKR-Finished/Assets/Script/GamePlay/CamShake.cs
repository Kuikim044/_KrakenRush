using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CamShake : MonoBehaviour
{
    public static CamShake instance { get; private set; }

    private CinemachineVirtualCamera mCam;
    private float shakeTime;
    private float shakeTimeTotal;
    private float startingIntensity;

    private void Awake()
    {
        instance = this;
        mCam= GetComponent<CinemachineVirtualCamera>();

    }

    public void ShakeCam(float intensidity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            mCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain= intensidity;
        startingIntensity = intensidity;
        shakeTimeTotal = time;
        shakeTime = time;
    }

    private void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime < 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    mCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1-(shakeTime/ shakeTimeTotal)));
            }
        }
    }
}

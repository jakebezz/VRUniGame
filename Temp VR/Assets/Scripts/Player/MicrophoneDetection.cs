using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneDetection : MonoBehaviour
{
    
    //REWATCH VIDEO AND COMMENT CODE, CHANGE IT TOO
    [SerializeField] private int sampleWindow = 64;
    [SerializeField] private AudioSource source;
    [SerializeField] private float loudnessSensibility = 100;
    [SerializeField] private float threshold = 0.1f;
    //the smaller the number the easier the detection
    [SerializeField] private float detectionNumber = 1;
    [SerializeField] private float loudness;

    private AudioClip microphoneClip;

    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {
        loudness = GetLoundnessFromMicroPhone(source.timeSamples, source.clip) * loudnessSensibility;

        if (loudness < threshold)
        {
            loudness = 0;
        }

        if (loudness >= detectionNumber)
        {
            Guard.alertedGuard = true;
        }
    }

    private void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    private float GetLoundnessFromMicroPhone(int clipPosition, AudioClip clip)
    {
        clipPosition = Microphone.GetPosition(Microphone.devices[0]);
        clip = microphoneClip;

        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}

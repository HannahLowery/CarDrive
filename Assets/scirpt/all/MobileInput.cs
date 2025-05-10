using UnityEngine;

public class MobileInput : MonoBehaviour
{
    [Header("Inputs")]
    public bool isAccelerating = false;
    public bool isBraking = false;

    [Header("Audio Sources")]
    public AudioSource gasAudioSource;
    public AudioSource brakeAudioSource;

    // Called by UI buttons
    public void PressGas()
    {
        isAccelerating = true;

        if (gasAudioSource != null && !gasAudioSource.isPlaying)
        {
            gasAudioSource.Play();
        }
    }

    public void ReleaseGas()
    {
        isAccelerating = false;

        if (gasAudioSource != null && gasAudioSource.isPlaying)
        {
            gasAudioSource.Stop();
        }
    }

    public void PressBrake()
    {
        isBraking = true;

        if (brakeAudioSource != null && !brakeAudioSource.isPlaying)
        {
            brakeAudioSource.Play();
        }
    }

    public void ReleaseBrake()
    {
        isBraking = false;

        if (brakeAudioSource != null && brakeAudioSource.isPlaying)
        {
            brakeAudioSource.Stop();
        }
    }
}

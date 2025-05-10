using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SignalBlinker : MonoBehaviour
{
    public Image leftSignalImage; // Left signal UI image
    public Image rightSignalImage; // Right signal UI image
    public float blinkSpeed = 0.5f; // seconds between blinks
    private bool isLeftBlinking = false;
    private bool isRightBlinking = false;
    private Coroutine leftBlinkCoroutine;
    private Coroutine rightBlinkCoroutine;

    public Animator leftSignalAnimator; // Left signal animator
    public Animator rightSignalAnimator; // Right signal animator

    public AudioSource signalAudioSource; // AudioSource for playing signal sound

    private void Start()
    {
        // Make sure both signals are off at the start
        leftSignalImage.enabled = true;
        rightSignalImage.enabled = true;
    }

    void Update()
    {
        // Listen for "left arrow" and "right arrow" key presses
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ToggleLeftSignal(); // Trigger the left signal toggle
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ToggleRightSignal(); // Trigger the right signal toggle
        }
    }

    // Toggle Left Signal
    public void ToggleLeftSignal()
    {
        if (isLeftBlinking)
        {
            if (leftBlinkCoroutine != null)
            {
                StopCoroutine(leftBlinkCoroutine); // Stop the left signal blinking coroutine
            }

            leftSignalAnimator.SetBool("IsBlinking", false); // Stop the left signal blinking animation
            isLeftBlinking = false;

            // Stop the audio when the signal stops
            signalAudioSource.Stop(); 
        }
        else
        {
            leftBlinkCoroutine = StartCoroutine(BlinkLeftSignal()); // Start the blinking coroutine
            leftSignalAnimator.SetBool("IsBlinking", true); // Start the left signal blinking animation
            isLeftBlinking = true;
            leftSignalImage.enabled = true; // Ensure the image is visible

            // Play the audio when the signal starts
            signalAudioSource.Play();
        }
    }

    // Toggle Right Signal
    public void ToggleRightSignal()
    {
        if (isRightBlinking)
        {
            if (rightBlinkCoroutine != null)
            {
                StopCoroutine(rightBlinkCoroutine); // Stop the right signal blinking coroutine
            }

            rightSignalAnimator.SetBool("IsBlinking", false); // Stop the right signal blinking animation
            isRightBlinking = false;

            // Stop the audio when the signal stops
            signalAudioSource.Stop(); 
        }
        else
        {
            rightBlinkCoroutine = StartCoroutine(BlinkRightSignal()); // Start the blinking coroutine
            rightSignalAnimator.SetBool("IsBlinking", true); // Start the right signal blinking animation
            isRightBlinking = true;
            rightSignalImage.enabled = true; // Ensure the image is visible

            // Play the audio when the signal starts
            signalAudioSource.Play();
        }
    }

    // Toggle Hazard Lights
    public void ToggleHazardLights()
    {
        if (isLeftBlinking && isRightBlinking)
        {
            // If both signals are blinking, turn them off
            if (leftBlinkCoroutine != null)
            {
                StopCoroutine(leftBlinkCoroutine);
            }

            if (rightBlinkCoroutine != null)
            {
                StopCoroutine(rightBlinkCoroutine);
            }

            leftSignalAnimator.SetBool("IsBlinking", false);
            rightSignalAnimator.SetBool("IsBlinking", false);
            isLeftBlinking = false;
            isRightBlinking = false;

            leftSignalImage.enabled = true;
            rightSignalImage.enabled = true;

            // Stop the audio when both signals stop
            signalAudioSource.Stop();
        }
        else
        {
            // Turn on hazard lights (both signals on)
            leftBlinkCoroutine = StartCoroutine(BlinkLeftSignal());
            rightBlinkCoroutine = StartCoroutine(BlinkRightSignal());
            leftSignalAnimator.SetBool("IsBlinking", true);
            rightSignalAnimator.SetBool("IsBlinking", true);
            isLeftBlinking = true;
            isRightBlinking = true;

            leftSignalImage.enabled = true;
            rightSignalImage.enabled = true;

            // Play the audio when hazard lights are on
            signalAudioSource.Play();
        }
    }

    // Left Signal Blinking Coroutine
    private IEnumerator BlinkLeftSignal()
    {
        while (isLeftBlinking)
        {
            leftSignalImage.enabled = !leftSignalImage.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    // Right Signal Blinking Coroutine
    private IEnumerator BlinkRightSignal()
    {
        while (isRightBlinking)
        {
            rightSignalImage.enabled = !rightSignalImage.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    public bool GetIsLeftBlinking()
    {
        return isLeftBlinking;
    }

    public bool GetIsRightBlinking()
    {
        return isRightBlinking;
    }
}

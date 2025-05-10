using UnityEngine;

public class LaneChangeDetector : MonoBehaviour
{
    public SignalBlinker signalBlinker;  // Reference to SignalBlinker script
    public CoinManager coinManager;      // Reference to CoinManager script
    public ProgressBar progressBar;     // Reference to the ProgressBar script (Slider)

    private bool isInLeftLane = false;  // Tracks if the player is in the left lane
    private bool isInRightLane = false; // Tracks if the player is in the right lane

    // Track if the player has already gained progress in a lane for the current glowing arrow
    private bool hasProgressedInLeftLane = false;
    private bool hasProgressedInRightLane = false;

    private void Update()
    {
        // Progress bar updates independently of signal logic
        // Check if the correct glowing arrow is on and the player is in the corresponding lane
        if (RandomArrowLighting.isLeftArrowGlowing && isInLeftLane && !hasProgressedInLeftLane)
        {
            // Increase progress if the player is in the left lane and the left arrow is glowing
            float newProgress = progressBar.GetCurrentProgress() + 0.1f;
            progressBar.UpdateProgress(newProgress);
            hasProgressedInLeftLane = true;  // Prevent double progress
        }

        if (RandomArrowLighting.isRightArrowGlowing && isInRightLane && !hasProgressedInRightLane)
        {
            // Increase progress if the player is in the right lane and the right arrow is glowing
            float newProgress = progressBar.GetCurrentProgress() + 0.1f;
            progressBar.UpdateProgress(newProgress);
            hasProgressedInRightLane = true;  // Prevent double progress
        }
    }

    // When the player enters a lane
    private void OnTriggerEnter(Collider other)
    {
        // Detect left lane
        if (other.CompareTag("LeftLane"))
        {
            isInLeftLane = true;

            // Increase progress bar only if the left arrow is glowing and no progress has been made yet
            if (RandomArrowLighting.isLeftArrowGlowing && !hasProgressedInLeftLane)
            {
                float newProgress = progressBar.GetCurrentProgress() + 0.1f;
                progressBar.UpdateProgress(newProgress);  // Increase progress by 10%
                hasProgressedInLeftLane = true;  // Set flag to prevent double progress
            }

            // Coin collection: Check if the left signal is on (signal lights only for coin collection)
          if (signalBlinker.GetIsLeftBlinking())
            {
                coinManager.AddCoin();
                signalBlinker.ToggleLeftSignal(); // Automatically turn off the left signal
                Debug.Log("Coin added for correct lane change (Left).");
            }
            else
            {
                Debug.Log("No signal, no coin (Left).");
            }
        }

        // Detect right lane
        if (other.CompareTag("RightLane"))
        {
            isInRightLane = true;

            // Increase progress bar only if the right arrow is glowing and no progress has been made yet
            if (RandomArrowLighting.isRightArrowGlowing && !hasProgressedInRightLane)
            {
                float newProgress = progressBar.GetCurrentProgress() + 0.1f;
                progressBar.UpdateProgress(newProgress);  // Increase progress by 10%
                hasProgressedInRightLane = true;  // Set flag to prevent double progress
            }

            // Coin collection: Check if the right signal is on (signal lights only for coin collection)
            if (signalBlinker.GetIsRightBlinking())
                {
                coinManager.AddCoin();
                signalBlinker.ToggleRightSignal(); // Automatically turn off the right signal
                 Debug.Log("Coin added for correct lane change (Right).");
                }
            else
            {
                Debug.Log("No signal, no coin (Right).");
            }
        }
    }

    // When the player exits the lane
    private void OnTriggerExit(Collider other)
    {
        // Reset lane status when leaving the lane
        if (other.CompareTag("LeftLane"))
        {
            isInLeftLane = false;
        }

        if (other.CompareTag("RightLane"))
        {
            isInRightLane = false;
        }
    }

    // Call this method when the arrow light changes to reset progress tracking
    public void ResetProgressFlags()
    {
        hasProgressedInLeftLane = false;
        hasProgressedInRightLane = false;
    }
}

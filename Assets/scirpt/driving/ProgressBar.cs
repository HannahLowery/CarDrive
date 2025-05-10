using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider progressSlider;  // Reference to the Slider component that acts as the progress bar
    private float progress = 0f;   // Current progress (0 to 1)

    // Update the progress
    public void UpdateProgress(float value)
    {
        // Only update if the new value is greater than the current progress
        if (value > progress)
        {
            progress = Mathf.Clamp01(value);  // Make sure the value stays between 0 and 1
            progressSlider.value = progress;  // Set the value of the slider (this updates the fill amount of the slider)
        }
    }

    // Get the current progress
    public float GetCurrentProgress()
    {
        return progressSlider.value;
    }

    // Reset the progress bar to 0 (optional, if you need to reset it manually)
    public void ResetProgress()
    {
        progress = 0f;
        progressSlider.value = progress;
    }
}

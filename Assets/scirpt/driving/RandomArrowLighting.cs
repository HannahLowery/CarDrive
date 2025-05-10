using UnityEngine;
using System.Collections;

public class RandomArrowLighting : MonoBehaviour
{
    public Animator leftArrowAnimator;  
    public Animator rightArrowAnimator; 
    public float minTime = 1f; 
    public float maxTime = 3f;

    // Reference to the LaneChangeDetector
    public LaneChangeDetector laneChangeDetector;

    public static bool isLeftArrowGlowing = false; // Track if left arrow is glowing
    public static bool isRightArrowGlowing = false; // Track if right arrow is glowing

    private void Start()
    {
        StartCoroutine(RandomArrowLightingCoroutine());
    }

    private IEnumerator RandomArrowLightingCoroutine()
    {
        while (true)
        {
            float randomInterval = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomInterval);

            if (Random.value > 0.5f)
            {
                LightUpLeftArrow();
            }
            else
            {
                LightUpRightArrow();
            }
        }
    }

    private void LightUpLeftArrow()
    {
        leftArrowAnimator.SetBool("glow", true);
        rightArrowAnimator.SetBool("glow", false);
        isLeftArrowGlowing = true;  // Set left arrow as glowing
        isRightArrowGlowing = false;  // Set right arrow as not glowing

        // Reset progress flags
        laneChangeDetector.ResetProgressFlags(); // Ensure laneChangeDetector is assigned in the Inspector
    }

    private void LightUpRightArrow()
    {
        rightArrowAnimator.SetBool("glow", true);
        leftArrowAnimator.SetBool("glow", false);
        isRightArrowGlowing = true;  // Set right arrow as glowing
        isLeftArrowGlowing = false;  // Set left arrow as not glowing

        // Reset progress flags
        laneChangeDetector.ResetProgressFlags(); // Ensure laneChangeDetector is assigned in the Inspector
    }
}

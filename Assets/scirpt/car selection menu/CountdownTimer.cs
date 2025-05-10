using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // The TMP text component to display the timer
    public GameObject gameOverPanel;   // The Game Over Panel that will be shown when time runs out
    public float timeRemaining = 30f;  // Time in seconds to count down from

    private bool isGameOver = false;

    void Start()
    {
        // Set the initial timer text
        UpdateTimerText();
    }

    void Update()
    {
        // Only count down if the game isn't over
        if (!isGameOver)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;  // Clamp the timer to 0
                GameOver();          // Trigger the game over screen
            }

            // Update the timer text with the remaining time
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        // Update the text to show the remaining time
        timerText.text = Mathf.Ceil(timeRemaining).ToString();
    }

    void GameOver()
    {
        // If game over panel is assigned, show it
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;  // Pause the game (optional)
        }

        isGameOver = true;  // Set game over flag to true to stop the countdown
    }
}

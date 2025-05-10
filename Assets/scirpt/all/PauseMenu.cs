using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Assign your pause panel in the Inspector

    public void PauseGame()
    {
        pausePanel.SetActive(true);  // Show the pause panel
        Time.timeScale = 0f;         // Pause the game
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Hide the pause panel
        Time.timeScale = 1f;         // Resume the game
    }
}

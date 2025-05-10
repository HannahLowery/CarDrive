using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public Button retryButton;
    public Button carSelectButton;
    public Button quitButton;

    private int carIndex;
    private int maxLives = 3;

    void Start()
    {
        // Get selected car index
        carIndex = PlayerPrefs.GetInt("carIndex", 0);

        // Check damage level
        int carDamage = PlayerPrefs.GetInt($"car{carIndex}_damage", maxLives);

        // Retry is only interactable if damage is greater than 0
        if (retryButton != null)
            retryButton.interactable = carDamage > 0;

        // Assign listeners
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);

        if (carSelectButton != null)
            carSelectButton.onClick.AddListener(ReturnToCarSelection);
    }

    void Retry()
    {
        Time.timeScale = 1f; // Resume game in case it was paused
        SceneManager.LoadScene("CarDrivee");
    }

    void ReturnToCarSelection()
    {
        Time.timeScale = 1f; // Resume game in case it was paused
        SceneManager.LoadScene("CarSelection");
    }
}


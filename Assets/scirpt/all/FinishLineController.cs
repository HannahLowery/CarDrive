using UnityEngine;
using UnityEngine.UI;

public class FinishLineController : MonoBehaviour
{
    public Slider progressSlider;         // Assign in Inspector
    public GameObject finishLine;         // Assign the Finish Line object
    public Transform carTransform;        // Your car's transform
    public GameObject gameWinPanel;       // Assign your Game Win UI panel
    public float spawnDistance = 3f;
    public float winDistanceThreshold = 2f; // Distance to trigger game win

    private bool finishSpawned = false;
    private bool gameWon = false;

    void Update()
    {
        // Spawn the finish line when progress is full
        if (!finishSpawned && progressSlider.value >= 1f)
        {
            Vector3 spawnPos = carTransform.position + carTransform.forward * spawnDistance;
            spawnPos.y = carTransform.position.y; // Keep at car’s height

            finishLine.transform.position = spawnPos;
            finishLine.SetActive(true);
            finishSpawned = true;

            Debug.Log("Finish line spawned in front of car.");
        }

        // Check distance to trigger game win
        if (finishSpawned && !gameWon)
        {
            float distance = Vector3.Distance(carTransform.position, finishLine.transform.position);
            if (distance <= winDistanceThreshold)
            {
                gameWon = true;
                if (gameWinPanel != null)
                {
                    gameWinPanel.SetActive(true);
                    Time.timeScale = 0f; // Optional: pause the game
                    Debug.Log("Game Won! Panel activated.");
                }
            }
        }
    }
}

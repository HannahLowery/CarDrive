using UnityEngine;

public class Collect : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    void Start()
    {
        // Initialize lives based on PlayerPrefs
        currentLives = maxLives;

        int index = PlayerPrefs.GetInt("carIndex", 0);
        int savedDamage = PlayerPrefs.GetInt($"car{index}_damage", maxLives);
        currentLives = savedDamage;

        Debug.Log("Starting with " + currentLives + " lives.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name); // Log the object colliding with the car

        if (collision.gameObject.CompareTag("Damage"))
        {
            Debug.Log("Damage tag detected!");

            // Apply damage
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Crash"))
        {
            Debug.Log("Crash tag detected!");

            // Handle crash case (if needed)
            TakeDamage(2); // or handle differently
        }
    }

    void TakeDamage(int amount)
    {
        currentLives -= amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        Debug.Log("Car took damage. Lives left: " + currentLives);

        // Save the updated lives to PlayerPrefs
        int index = PlayerPrefs.GetInt("carIndex", 0);
        PlayerPrefs.SetInt($"car{index}_damage", currentLives);
        PlayerPrefs.Save();

        if (currentLives <= 0)
        {
            Debug.Log("Car destroyed!");
            // Handle game over logic if needed
        }
    }
}


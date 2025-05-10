using UnityEngine;
using TMPro;

public class CarCollisionHandler : MonoBehaviour
{
    public Rigidbody carRigidbody; // Assign your car's Rigidbody in the Inspector
    public float bounceForce = 500f; // Adjust this to control bounce strength
    public GameObject gameOverScreen; // Assign your Game Over UI panel here
    public int maxDamages = 3; // Max damage count before game over
    private int currentDamages = 0; // Current damage count

    public TextMeshProUGUI damageMessageText; // Assign your TextMeshPro text in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // Bounce the car back
            Vector3 bounceDirection = -transform.forward;
            carRigidbody.AddForce(bounceDirection * bounceForce);

            // Increase the damage count
            currentDamages++;

            // Show appropriate damage message
            if (damageMessageText != null)
            {
                if (currentDamages < maxDamages)
                {
                    // Show remaining lives
                    damageMessageText.text = $"You have {maxDamages - currentDamages} lives left!";
                    Invoke(nameof(ClearDamageMessage), 2f);
                }
                else
                {
                    // Show destruction message on third hit
                    damageMessageText.text = "Your car has been destroyed!";
                    Invoke(nameof(TriggerGameOver), 1f); // Wait before triggering game over
                }
            }
        }
        else if (collision.gameObject.CompareTag("Crash") || collision.gameObject.CompareTag("Car"))
        {
            // If the car crashes, show crash message and trigger game over instantly
            if (damageMessageText != null)
            {
                damageMessageText.text = "You crashed!!";
            }
            Invoke(nameof(TriggerGameOver), 1f); // Delay before triggering game over
        }
    }

    private void TriggerGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }

    private void ClearDamageMessage()
    {
        if (damageMessageText != null)
        {
            damageMessageText.text = "";
        }
    }
}

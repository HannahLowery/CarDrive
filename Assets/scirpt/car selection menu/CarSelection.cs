using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CarSelection : MonoBehaviour
{
    public GameObject[] cars;
    public Button next;
    public Button prev;
    public Button repairButton;
    public Button raceButton;
    public Slider damageSlider;
    public TextMeshProUGUI messageText;
    public AudioSource hornAudio; // 🔊 Add this in Inspector

    int index;
    int maxLives = 3;
    int repairCost = 3;

    void Start()
    {
        index = PlayerPrefs.GetInt("carIndex");
        Debug.Log("Loaded saved car index: " + index);

        for (int i = 0; i < cars.Length; i++)
            cars[i].SetActive(false);

        if (index >= 0 && index < cars.Length)
        {
            cars[index].SetActive(true);
            Debug.Log("Activated car at index: " + index);
        }
        else
        {
            index = 0;
            cars[0].SetActive(true);
        }

        UpdateDamageSlider();

        if (repairButton != null)
            repairButton.onClick.AddListener(RepairDamage);

        if (raceButton != null)
            raceButton.onClick.AddListener(Race);
    }

    void Update()
    {
        next.interactable = index < cars.Length - 1;
        prev.interactable = index > 0;

        if (raceButton != null && damageSlider != null)
            raceButton.interactable = damageSlider.value > 0;
    }

    public void Next()
    {
        if (index < cars.Length - 1)
        {
            index++;
            UpdateCarDisplay();
        }
    }

    public void Prev()
    {
        if (index > 0)
        {
            index--;
            UpdateCarDisplay();
        }
    }

    public void Race()
    {
        int damage = PlayerPrefs.GetInt($"car{index}_damage", maxLives);
        if (damage > 0)
        {
            StartCoroutine(PlayHornAndLoadScene());
        }
        else
        {
            ShowMessage("Repair your car to race!");
        }
    }

    private IEnumerator PlayHornAndLoadScene()
    {
        if (hornAudio != null)
        {
            hornAudio.Play();
            yield return new WaitForSeconds(hornAudio.clip.length); // ⏱ Wait for horn to finish
        }

        SceneManager.LoadSceneAsync("CarDrivee");
    }

    private void UpdateCarDisplay()
    {
        for (int i = 0; i < cars.Length; i++)
            cars[i].SetActive(false);

        if (index >= 0 && index < cars.Length)
            cars[index].SetActive(true);

        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();

        UpdateDamageSlider();
    }

    private void UpdateDamageSlider()
    {
        if (damageSlider != null)
        {
            int savedDamage = PlayerPrefs.GetInt($"car{index}_damage", maxLives);
            damageSlider.maxValue = maxLives;
            damageSlider.value = savedDamage;

            if (raceButton != null)
                raceButton.interactable = savedDamage > 0;
        }
    }

    public void RepairDamage()
    {
        int currentDamage = PlayerPrefs.GetInt($"car{index}_damage", maxLives);
        int coinCount = PlayerPrefs.GetInt("coinCount", 0);

        if (currentDamage < maxLives && coinCount >= repairCost)
        {
            currentDamage++;
            coinCount -= repairCost;

            PlayerPrefs.SetInt($"car{index}_damage", currentDamage);
            PlayerPrefs.SetInt("coinCount", coinCount);
            PlayerPrefs.Save();

            UpdateDamageSlider();
            FindFirstObjectByType<CoinDisplay>().SendMessage("Start");

            ShowMessage("Repaired 1 damage!");
        }
        else
        {
            ShowMessage("Not enough coins or car is already fully repaired.");
        }
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            CancelInvoke(nameof(ClearMessage));
            Invoke(nameof(ClearMessage), 2f);
        }
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}

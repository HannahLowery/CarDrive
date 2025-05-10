using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform coinSpawnPoint;
    public int coinCount = 0;
    public float coinOffsetY = 50f;

    public AudioSource coinAudioSource; // Reference to your AudioSource

    private void Start()
    {
        coinCount = PlayerPrefs.GetInt("coinCount", 0);
    }

    public void AddCoin()
    {
        coinCount++;
        PlayerPrefs.SetInt("coinCount", coinCount);
        PlayerPrefs.Save();

        GameObject newCoin = Instantiate(coinPrefab, coinSpawnPoint);
        RectTransform coinRect = newCoin.GetComponent<RectTransform>();
        coinRect.anchoredPosition = new Vector2(0, coinOffsetY * coinCount);

        if (coinAudioSource != null)
        {
            coinAudioSource.Play();
        }
    }
}

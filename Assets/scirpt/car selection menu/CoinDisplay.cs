using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        int coinCount = PlayerPrefs.GetInt("coinCount", 0);
        coinText.text = "Coins: " + coinCount;
    }
}

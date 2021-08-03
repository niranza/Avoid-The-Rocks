using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private TextMeshProUGUI timerText;

    public void updateScoreText(int currentScore)
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void updateCoinText(int inGameCollectedCoins)
    {
        coinText.text = "Coins: " + inGameCollectedCoins.ToString();
    }

    public void updateTimerText(int trueTimer)
    {
        timerText.text = "Seconds: " + trueTimer.ToString();
    }
}

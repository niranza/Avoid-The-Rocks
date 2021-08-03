using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//only the scripts responsible for death communicate with this script
public class GameManager : MonoBehaviour
{
    [SerializeField] private MainUIController mainUI;

    [System.NonSerialized] public int inGameCollectedCoins;

    private bool restarted;

    private float timer;
    private int trueTimer;
    [SerializeField] private float gamePace = 0.4f;

    [SerializeField] private Transform player;
    private PlayerMovementByTouch playerMovement;
    private PlayerHealth playerHealth;
    private bool hasBoosted = false;
    [SerializeField] private int increaments = 15;
    void Awake()
    {
        playerMovement = player.GetComponent<PlayerMovementByTouch>();
        playerHealth = player.GetComponent<PlayerHealth>();
        timer = 0;
        inGameCollectedCoins = 0;
        restarted = false;
    }

    void Update()
    {
        int currentScore = (int)player.position.z;
        mainUI.updateScoreText(currentScore);


        timer += Time.deltaTime;
        trueTimer = (int)timer;
        mainUI.updateTimerText(trueTimer);
        if (trueTimer != 0 && trueTimer % increaments == 0)
            StartCoroutine(boostByTimer());
    }

    private IEnumerator boostByTimer()
    {
        if (!hasBoosted)
        {
            boostPlayerMovementSpeed();
            hasBoosted = true;
            yield return new WaitForSeconds(1.1f);
            hasBoosted = false;
        }
    }
    private void boostPlayerMovementSpeed()
    {
        playerMovement.forwardSpeed += (8f * gamePace) / playerMovement.forwardSpeed;
    }
    public void restart()
    {
        if (!restarted)
        {
            restarted = true;
            playerHealth.die();
            StateNameController.TOTAL_COINS_AMOUNT += inGameCollectedCoins;
            SaveSystem.savePlayer();
            StartCoroutine(reloadScene());
        }
    }
    private IEnumerator reloadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game");
    }

    public void updateCoins()
    {
        inGameCollectedCoins++;
        mainUI.updateCoinText(inGameCollectedCoins);
    }
}

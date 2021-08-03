using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTriggerBox : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        GameObject gameManagerObj = GameObject.Find("Game Manager");
        if (gameManagerObj != null) gameManager = gameManagerObj.GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.updateCoins();
            transform.parent.gameObject.SetActive(false);
        }
    }
}

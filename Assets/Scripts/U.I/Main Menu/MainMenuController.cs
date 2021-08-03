using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private SpawnModels modelSpawner;
    private void Start()
    {
        if (coinText != null) coinText.text = StateNameController.TOTAL_COINS_AMOUNT.ToString();
    }
    public void startGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
    public void selectCharacter(string gfx)
    {
        int num = 0;
        switch (gfx)
        {
            case "Cyborg":
                num = 0;
                break;
            case "Alien":
                num = 1;
                break;
        }
        modelSpawner.setModel(num);
        StateNameController.PLAYER_SELETED_GFX = num; 
    }
}
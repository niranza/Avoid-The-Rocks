using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    private static bool NEED_LOAD = true;
    private void Awake()
    {
        if (NEED_LOAD) load();
    }
    private void load()
    {
        if (SaveSystem.loadPlayer() == null)
            SaveSystem.savePlayer();
        else
        {
            PlayerData playerData = SaveSystem.loadPlayer();
            StateNameController.TOTAL_COINS_AMOUNT = playerData.coins;
            StateNameController.PLAYER_SELETED_GFX = playerData.playerSelectedGfx;
        }
        NEED_LOAD = false;
    }
}

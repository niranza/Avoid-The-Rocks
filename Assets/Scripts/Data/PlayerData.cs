using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins;
    public int playerSelectedGfx;
    public PlayerData()
    {
        coins = StateNameController.TOTAL_COINS_AMOUNT;
        playerSelectedGfx = StateNameController.PLAYER_SELETED_GFX;
    }
}

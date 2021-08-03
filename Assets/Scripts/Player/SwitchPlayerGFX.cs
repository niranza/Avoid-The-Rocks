using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerGFX : MonoBehaviour
{
    private Enums.playerGfx selectedGfx = Enums.playerGfx.cyborg;

    private void Awake()
    {
        selectedGfx = (Enums.playerGfx)StateNameController.PLAYER_SELETED_GFX;
        selectGfx();
    }

    private void selectGfx()
    {
        int i = 0;
        foreach(Transform gfx in transform)
        {
            if (i == (int)selectedGfx)
                gfx.gameObject.SetActive(true);
            else
                gfx.gameObject.SetActive(false);
            i++;
        }
    }
}
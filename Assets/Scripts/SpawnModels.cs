using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnModels : MonoBehaviour
{
    [SerializeField] private List<GameObject> models;
    private int previousNum;
    private GameObject previousModel;
    private void Start()
    {
        previousNum = StateNameController.PLAYER_SELETED_GFX;
        previousModel = Instantiate(models[previousNum]);
    }
    public void setModel(int num)
    {
        if (previousNum != num)
        {
            if (previousModel != null)
                Destroy(previousModel);
            previousModel = Instantiate(models[num]);
            previousNum = num;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Transform ground;
    [SerializeField] private Transform triggerBoxTransform;
    private Vector3 startingPos;
    private GameController gameController;
    private float maxRange;
    private bool sceneStarted;
    private void Awake()
    {
        sceneStarted = false;
        gameController = Utils.findComponent<GameController>("Game Controller");
        maxRange = (ground.localScale.x * 5) - (triggerBoxTransform.localScale.z / 2f);
    }
    private void OnEnable()
    {
        startingPos = transform.position;
        startingPos.x = getStartingPositionX();
        transform.position = startingPos;
        gameController.setLastCoinPosition(transform.position);
        sceneStarted = true;
    }
    private float getStartingPositionX()
    {
        if (gameController != null && sceneStarted)
            return Mathf.Clamp(gameController.getRandomSpot(), -maxRange, maxRange);
        else return Random.Range(-maxRange, maxRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] private Transform middlePoint;
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    private PlayerMovementByTouch playerMovement;
    private Vector3 startingPos;
    private Vector3 middlePos;
    private bool ableToMove;
    public void setAbleToMove(bool boolean)
    {
        ableToMove = boolean;
    }
    private void Awake()
    {
        playerMovement = player.GetComponent<PlayerMovementByTouch>();
    }
    private void Start()
    {
        ableToMove = true;
        speed *= .01f;
        startingPos = transform.position;
        middlePos = (startingPos + player.position) / 2f;
        middlePoint.position = middlePos;
    }

    private void Update()
    {
        if (ableToMove)
            transform.Translate(Vector3.forward * Time.deltaTime * speed * playerMovement.forwardSpeed);
    }

    public void moveToMidPoint(Vector3 currentPlayerPosition)
    {
        if (transform.position.z < middlePoint.position.z)
        {
            transform.position = middlePoint.position;
            //add animation here. the enemies GFX lerp to the player
        }
        else
        {
            transform.position = currentPlayerPosition;
        }
    }

    private void checkForStop()
    {
        if (transform.position.z > middlePoint.position.z)
            ableToMove = false;
    }

    public void enemiesBack()
    {
        ableToMove = false;
        Vector3 positiontoReturn = player.position;
        positiontoReturn.z += startingPos.z;
        transform.position = positiontoReturn;
        //play enemies hit animation
        ableToMove = true;
    }
}

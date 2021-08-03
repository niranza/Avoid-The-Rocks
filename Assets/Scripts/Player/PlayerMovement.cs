using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float movementSpeed;
    private Vector3 vectorToMove;
    private Rigidbody rb;
    private PlayerMovementByTouch touchMovement;

    //animation
    [SerializeField] private Transform gfx;
    private Vector3 rotateTo;
    private Quaternion newRotation;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        touchMovement = GetComponent<PlayerMovementByTouch>();
    }
    private void Start()
    {
        rotateTo = Vector3.zero;
        movementSpeed = 10;
    }
    void Update()
    {
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        vectorToMove.x = x;
        vectorToMove.z = touchMovement.forwardSpeed / 10f;

        //animation
        rotateTo.y = x * 90;
        newRotation = Quaternion.Euler(rotateTo);
        gfx.transform.rotation = Quaternion.Slerp(gfx.transform.rotation, newRotation, Time.deltaTime * 8);

        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(touchMovement.enableSlice());
            rb.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            touchMovement.swipedUp = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            touchMovement.swipedDown = true;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (vectorToMove * movementSpeed * Time.fixedDeltaTime));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementByTouch : MonoBehaviour
{
    [Header("Movement")]
    //movement logic
    public float forwardSpeed = 6f;
    [SerializeField] private float speedSensetivity = 25f;
    [SerializeField] private float devidingHorizontalScreen = 4.5f;
    private float pixelsForHorizontalSide;
    private Vector3 directionToMove;
    private float startingX;
    private float unitsToMoveInPixels;
    private float x = 0;
    private Rigidbody rb;
    
    [Header("Rotation")]
    //rotation by movement
    [SerializeField] private Transform gfx;
    [SerializeField] private int maxRotationAngle = 75;
    [SerializeField] private float slerpToRotationTime = 10;
    private Vector3 rotateTo;
    private Quaternion newRotation;

    [Header("Slice")]
    //single screen tap
    [SerializeField] private ParticleSystem readyToHitPS;
    [SerializeField] private float maxQuickTouchLength = 0.15f;
    public float waitForHitDuration = 0.3f;
    [System.NonSerialized]
    public bool isSlicing;
    private float quickTouchTimer;
    private bool countTimer;

    [Header("Jump")]
    //Swipe Up
    [SerializeField] private float devidingVerticalScreen = 30f; //swipping sensetivity
    private float currentY;
    private float lastY;
    private float pixelsForVerticalSide;
    private float pixelVelocity;
    [System.NonSerialized]
    public bool swipedUp;

    //jump
    private bool isGrounded;
    [SerializeField] private float jumpForce = 400;

    [Header("Duck")]
    //Swipe Down
    [System.NonSerialized]
    public bool swipedDown;

    //duck
    [SerializeField] private float duckDuration = 1f;
    private bool isDucking;
    private CapsuleCollider coll;
    Vector3 originalCollCenter;
    Vector3 newCollCenter;
    int originalCollDirection;

    //animation
    private Animator animator;
    const string JUMP_UP = "jump_up";
    const string JUMP_DOWN = "jump_down";
    const string SLIDE = "slide";

    [Header("Debugging")]
    //testing this
    [SerializeField] private Text testText;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        //called on start because player needs to be configured on awake at SwitchPlayerGFX
        animator = GetComponentInChildren<Animator>();

        pixelsForHorizontalSide = Screen.width / devidingHorizontalScreen;
        quickTouchTimer = 0;
        countTimer = false;
        isSlicing = false;
        pixelsForVerticalSide = Screen.height / devidingVerticalScreen;
        swipedUp = false;
        swipedDown = false;
        isDucking = false;
        rotateTo = Vector3.zero;
        jumpForce *= rb.mass;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            bool twoFingers = false;
            if (Input.touchCount > 1) twoFingers = true;

            Touch t = Input.GetTouch(0);

            if (twoFingers)
            {
                Touch t1 = Input.GetTouch(1);

                if (t1.phase == TouchPhase.Began) startQuickTouchTimer();
                if (t1.phase == TouchPhase.Ended) endQuickTouchTimer();
            }

            if (t.phase == TouchPhase.Began)
            {
                //swipe up & down
                lastY = t.position.y;

                if (!twoFingers) startQuickTouchTimer();

                startingX = t.position.x;
            }

            if (t.phase == TouchPhase.Moved)
            {
                //getting amount of pixels in certain direction and clamping them
                unitsToMoveInPixels = Mathf.Clamp(t.position.x - startingX, -pixelsForHorizontalSide, pixelsForHorizontalSide);
                //setting the value between 0 and 1
                x = unitsToMoveInPixels * (1f / pixelsForHorizontalSide);
                //multypling with speed sensetivity
                directionToMove.x = x * speedSensetivity;

                //swipe up & down
                currentY = t.position.y;
                pixelVelocity = currentY - lastY;
                if (pixelVelocity > pixelsForVerticalSide) swipedUp = true;
                if (pixelVelocity < -pixelsForVerticalSide) swipedDown = true;
                lastY = currentY;
                //make sure quick swipe doesn't activate slicing
                if (!twoFingers) quickTouchTimer = maxQuickTouchLength;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (!twoFingers) endQuickTouchTimer();

                directionToMove.x = 0f;
                x = 0;
            }
        }
        directionToMove.z = forwardSpeed;

        //rotations
        rotateTo.y = x * maxRotationAngle;
        newRotation = Quaternion.Euler(rotateTo);
        gfx.transform.rotation = Quaternion.Slerp(gfx.transform.rotation, newRotation, Time.deltaTime * slerpToRotationTime);

        //timer
        if (countTimer) quickTouchTimer += Time.deltaTime;
        if (quickTouchTimer > maxQuickTouchLength) countTimer = false;

        //ground checks
        if (transform.position.y <= 0) isGrounded = true;
        else isGrounded = false;

        //animation
        if (isGrounded)
        {
            setVelocityZero();
            animator.SetBool(JUMP_UP, false);
            animator.SetBool(JUMP_DOWN, true);
        }
        if (!isGrounded)
        {
            animator.SetBool(JUMP_DOWN, false);
            animator.SetBool(JUMP_UP, true);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (directionToMove * Time.fixedDeltaTime));

        //jump
        if (swipedUp && isGrounded) jump();
        swipedUp = false;
        if (swipedDown && !isDucking) duck();
        swipedDown = false;
    }
    private void startQuickTouchTimer()
    {
        quickTouchTimer = 0;
        countTimer = true;
    }
    private void endQuickTouchTimer()
    {
        countTimer = false;
        if (quickTouchTimer < maxQuickTouchLength)
            StartCoroutine(enableSlice());
    }

    public IEnumerator enableSlice()
    {
        isSlicing = true;
        readyToHitPS.Play();
        yield return new WaitForSeconds(waitForHitDuration);
        readyToHitPS.Stop();
        isSlicing = false;
    }

    private void jump()
    {
        StartCoroutine(enableSlice());
        if (isDucking) endDucking();
        rb.AddForce(0, jumpForce, 0);
    }

    #region Ducking
    private void duck()
    {
        StartCoroutine(duckCoroutine());
    }
    private IEnumerator duckCoroutine()
    {
        startDucking();
        yield return new WaitForSeconds(duckDuration);
        endDucking();
    }
    private void startDucking()
    {
        animator.SetBool(SLIDE, true);
        StartCoroutine(enableSlice());
        if (!isGrounded) rb.AddForce(0, -jumpForce * 2, 0);
        isDucking = true;
        originalCollDirection = coll.direction;
        coll.direction = 2;
        originalCollCenter = coll.center;
        newCollCenter = coll.center;
        newCollCenter.y = 0.5f;
        coll.center = newCollCenter;
    }
    private void endDucking()
    {
        coll.center = originalCollCenter;
        coll.direction = originalCollDirection;
        animator.SetBool(SLIDE, false);
        isDucking = false;
    }
    #endregion

    private void setVelocityZero()
    {
        rb.velocity = Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BreakingMeteor : MonoBehaviour
{
    [SerializeField] private bool useCameraShakeWhenHit;

    //meteor
    private Rigidbody rb;
    private SphereCollider sphereColl;
    private MeshRenderer meshRenderer;

    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 meteorOffSetFromDestination;
    [SerializeField] private float spawnHeight = 13f;
    [SerializeField] private List<GameObject> trails;
    [SerializeField] private float explosionDelay;
    private float[] xPositions = new float[] { 3.5f, -3.5f };
    private Vector3 hitPos;
    private float constantSpeed;
    private float meteorDistance;
    private bool ableToMove;

    //pools
    private ObjectPooler impactEffectPool;
    private ObjectPooler brokenGroundPool;

    //player
    private PlayerMovementByTouch player;
    private float playerMovementSpeed = 10;

    //single trap
    [SerializeField] private bool spawmRandomly = true;

    //game controller
    private GameController gameController;
    private bool sceneStarted;

    private void Awake()
    {
        sceneStarted = true;

        rb = GetComponent<Rigidbody>();
        sphereColl = GetComponent<SphereCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        player = Utils.findComponent<PlayerMovementByTouch>("Player");

        impactEffectPool = Utils.findComponent<ObjectPooler>("Large Impact Effect Pool");

        brokenGroundPool = Utils.findComponent<ObjectPooler>("Broken Ground Pool");

        gameController = Utils.findComponent<GameController>("Game Controller");
    }
    void OnEnable()
    {
        //enable movement
        ableToMove = true;

        resetVelocity();

        hitPos = getHitPos();

        transform.position = getStartingPos();

        transform.LookAt(hitPos);

        constantSpeed = getConstantSpeed();

        sceneStarted = false;
    }

    #region OnEnableMethods
    private void resetVelocity()
    {
        rb.velocity = Vector3.zero;
    }
    private Vector3 getHitPos()
    {
        float delay = (20f / playerMovementSpeed);
        if (spawmRandomly)
        {
            int randomNum = Random.Range(0, 2);
            if (!sceneStarted)
                switch (randomNum)
                {
                    case 0:
                        gameController.removeAvailableSpots(-3, 4, delay);
                        break;
                    case 1:
                        gameController.removeAvailableSpots(-3, 4, delay);
                        break;
                }

            return new Vector3(xPositions[randomNum],
                transform.position.y,
                transform.position.z
                );
        }
        else return transform.position;
    }
    private Vector3 getStartingPos()
    {
        float randomX = Random.Range(spawnHeight / 2f, spawnHeight);
        float randomY = spawnHeight;
        float zModifier = spawnHeight / 5f;
        float randomZ = Random.Range(zModifier, spawnHeight);
        Vector3 offSet = new Vector3(randomX, randomY, randomZ);
        Vector3 startingPos = hitPos + offSet;
        return startingPos;
    }
    private float getConstantSpeed()
    {
        meteorDistance = Vector3.Distance(hitPos + meteorOffSetFromDestination, transform.position);
        if (player != null)
        {
            playerMovementSpeed = player.forwardSpeed;
            return meteorDistance * playerMovementSpeed * speed;
        }
        else return meteorDistance * speed * 10;
    }
    #endregion

    void FixedUpdate()
    {
        //moving forward
        if (ableToMove)
            rb.MovePosition(transform.position + (transform.forward * constantSpeed * Time.fixedDeltaTime));
    }


    private void OnCollisionEnter(Collision other)
    {
        sphereColl.enabled = false;
        meshRenderer.enabled = false;
        ableToMove = false;
        StartCoroutine(trigger(other));
    }

    #region OnCollision Enter Methods
    private IEnumerator trigger(Collision other)
    {
        //debugging
        Debug.Log("Collided with " + other.gameObject.name);

        yield return new WaitForSeconds(explosionDelay);

        ContactPoint contact = other.contacts[0];
        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

        //instantiating default impact effect
        if (impactEffectPool != null) impactEffectPool.getObject(pos, rot);

        //summoning ground
        Transform ground = other.transform.root;
        if (brokenGroundPool != null)
        {
            brokenGroundPool.getObject(ground.position, ground.rotation);
            ground.gameObject.SetActive(false);
        }

        //shake screen
        if (useCameraShakeWhenHit && CameraShaker.Instance != null)
            CameraShaker.Instance.ShakeOnce(1.5f, 1.5f, 0.1f, 1f);

        explode();
    }

    private void explode()
    {
        //trails PS
        float delay = 0;
        bool trailsValid = false;
        if (trails.Count > 0)
            for (int i = 0; i < trails.Count; i++)
                if (trails[i] != null && trails[i].activeInHierarchy)
                {
                    trailsValid = true;
                    ParticleSystemTrail trail = trails[i].GetComponent<ParticleSystemTrail>();
                    trail.setFalseWithDelay();
                    if (trail.delay > delay) delay = trail.delay;
                }

        //setting false this meteor
        if (trailsValid)
            StartCoroutine(setFalseWithDelay(delay));
        else setFalse();
    }
    private IEnumerator setFalseWithDelay(float delay)
    {
        //wait for particle system trail to end
        yield return new WaitForSeconds(delay);
        setFalse();
    }

    private void setFalse()
    {
        meshRenderer.enabled = true;
        sphereColl.enabled = true;

        gameObject.SetActive(false);
    }
    #endregion
}

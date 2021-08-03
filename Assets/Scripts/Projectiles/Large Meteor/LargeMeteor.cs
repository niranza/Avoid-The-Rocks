using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class LargeMeteor : MonoBehaviour
{
    [SerializeField] private bool useCameraShakeWhenHit;

    //meteor
    private Rigidbody rb;
    private SphereCollider sphereColl;
    private MeshRenderer meshRenderer;

    private Vector3 hitPos;
    private float[] xValues = new float[] { -2.5f, 2.5f };
    [SerializeField] private float spawnHeight = 13f;
    [SerializeField] private List<GameObject> trails;
    private float constantSpeed;
    private float meteorDistance;
    private bool ableToMove;

    //pools
    private ObjectPooler targetPool;
    private ObjectPooler impactEffectPool;

    //player
    private Transform player;
    private float playerMovementSpeed = 10f;
    [SerializeField] private int minDamage = 2;
    [SerializeField] private int maxDamage = 10;

    //target
    private GameObject target;
    private bool sceneStarted;
    [SerializeField] private Vector3 meteorOffSetFromTarget;
    [SerializeField] private Transform ground;

    //game controller
    private GameController gameController;
    private void Awake()
    {
        sceneStarted = true;

        rb = GetComponent<Rigidbody>();
        sphereColl = GetComponent<SphereCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        player = Utils.findComponent<Transform>("Player");

        targetPool = Utils.findComponent<ObjectPooler>("Large Target Pool");

        impactEffectPool = Utils.findComponent<ObjectPooler>("Large Impact Effect Pool");

        gameController = Utils.findComponent<GameController>("Game Controller");
    }
    void OnEnable()
    {
        //enable movement
        ableToMove = true;
        resetMeteorVelocity();

        hitPos = getMeteorHitPosition();

        spawnTarget(hitPos, Quaternion.identity);

        transform.position = getStartingPos();

        setMeteorRotationToTarget(hitPos);

        setMeteorConstantSpeed();

        sceneStarted = false;
    }

    #region OnEnable Methods
    private void resetMeteorVelocity()
    {
        rb.velocity = Vector3.zero;
    }
    private Vector3 getMeteorHitPosition()
    {
        int randomSide = Random.Range(0, 2);
        float delay = (5f / playerMovementSpeed) * 3f;
        if (!sceneStarted)
            switch (randomSide)
            {
                case 0:
                    gameController.removeAvailableSpots(-5, 0, delay);
                    break;
                case 1:
                    gameController.removeAvailableSpots(-1, 4, delay);
                    break;
            }
        Vector3 meteorSpawnPos = new Vector3(xValues[randomSide],
            transform.position.y,
            transform.position.z
            );
        return meteorSpawnPos;
    }
    private void spawnTarget(Vector3 position, Quaternion rotation)
    {
        if (targetPool != null && !sceneStarted)
            target = targetPool.getObject(position, rotation);
    }
    private Vector3 getStartingPos()
    {
        Vector3 offSet = new Vector3(0, spawnHeight * 2, 20);
        Vector3 startingPos = hitPos + offSet;
        return startingPos;
    }
    private void setMeteorRotationToTarget(Vector3 position)
    {
        transform.LookAt(position);
    }
    private void setMeteorConstantSpeed()
    {
        meteorDistance = Vector3.Distance(hitPos + meteorOffSetFromTarget, transform.position);
        if (player != null)
        {
            playerMovementSpeed = player.GetComponent<PlayerMovementByTouch>().forwardSpeed;
            float playerDistance = Vector3.Distance(hitPos, player.position);
            constantSpeed = meteorDistance * playerMovementSpeed / playerDistance;
        }
        else constantSpeed = meteorDistance / 2f;
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
        trigger(other);
    }

    #region OnCollisionEnter Methods
    private void trigger(Collision other)
    {
        ContactPoint contact = other.contacts[0];
        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

        if (other.gameObject.tag == "Player")
        {
            int damageToTake = Random.Range(minDamage, maxDamage);
            GameObject player = other.gameObject;

            //damaging the player and creating camera shake effect
            player.GetComponent<PlayerHealth>().takeDamage(damageToTake);
            if (useCameraShakeWhenHit && CameraShaker.Instance != null)
                CameraShaker.Instance.ShakeOnce(8f, 8f, 0.1f, 1f);
        }

        //instantiating default impact effect
        if (impactEffectPool != null) impactEffectPool.getObject(pos, rot);

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

        //setting false target
        if (target != null)
            target.SetActive(false);

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

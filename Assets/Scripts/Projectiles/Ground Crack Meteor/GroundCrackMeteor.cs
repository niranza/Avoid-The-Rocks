using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class GroundCrackMeteor : MonoBehaviour
{
    [SerializeField] private bool useCameraShakeWhenHit;

    //meteor
    private Rigidbody rb;
    private SphereCollider sphereColl;
    private MeshRenderer meshRenderer;

    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 meteorOffSetFromDestination;
    [SerializeField] private Transform ground;
    [SerializeField] private float spawnHeight = 13f;
    [SerializeField] private List<GameObject> trails;
    [SerializeField] private float explosionDelay;
    private Vector3 hitPos;
    private float constantSpeed;
    private float meteorDistance;
    private bool ableToMove;

    //pools
    private ObjectPooler impactEffectPool;
    private ObjectPooler electricGroundCrackPool;

    //player
    private PlayerMovementByTouch player;

    //single trap
    [SerializeField] private bool spawmRandomly = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sphereColl = GetComponent<SphereCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
            player = playerObject.GetComponent<PlayerMovementByTouch>();

        GameObject impactEffectPoolObject = GameObject.Find("Large Impact Effect Pool");
        if (impactEffectPoolObject != null)
            impactEffectPool = impactEffectPoolObject.GetComponent<ObjectPooler>();

        GameObject electricGroundCrackPoolObject = GameObject.Find("Electric Ground Crack Pool");
        if (electricGroundCrackPoolObject != null)
            electricGroundCrackPool = electricGroundCrackPoolObject.GetComponent<ObjectPooler>();
    }
    void OnEnable()
    {
        //enable movement
        ableToMove = true;

        //reseting meteors velocity
        rb.velocity = Vector3.zero;

        //reseting the hit position to the spawner's on random x
        if (spawmRandomly)
        {
            float maxRange = (ground.localScale.x * 5f) - (transform.localScale.x / 2f);
            hitPos = new Vector3(Random.Range(-maxRange, maxRange),
                transform.position.y,
                transform.position.z
                );
        }
        else hitPos = transform.position;

        //creating a random position to start at
        float randomX = Random.Range(spawnHeight / 2f, spawnHeight);
        float randomY = spawnHeight;
        float zModifier = spawnHeight / 5f;
        float randomZ = Random.Range(zModifier, spawnHeight);
        Vector3 offSet = new Vector3(randomX, randomY, randomZ);
        Vector3 startingPos = hitPos + offSet;
        transform.position = startingPos;

        //looking at the direction of the target
        transform.LookAt(hitPos);

        //reseting the constant speed accordingly with the player
        meteorDistance = Vector3.Distance(hitPos + meteorOffSetFromDestination, transform.position);
        if (player != null)
        {
            float playerMovementSpeed = player.forwardSpeed;
            constantSpeed = meteorDistance * playerMovementSpeed * speed;
        }
        else constantSpeed = meteorDistance * speed * 10;
    }
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
    private IEnumerator trigger(Collision other)
    {
        yield return new WaitForSeconds(explosionDelay);

        ContactPoint contact = other.contacts[0];
        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

        //instantiating default impact effect
        if (impactEffectPool != null) impactEffectPool.getObject(pos, rot);

        //instantiating jump above trigger
        if (electricGroundCrackPool != null) electricGroundCrackPool.getObject(hitPos, Quaternion.identity);

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
}

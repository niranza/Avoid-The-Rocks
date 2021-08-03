using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Meteor : MonoBehaviour
{
    [SerializeField] private bool useCameraShakeWhenHit;

    //meteor
    private Rigidbody rb;
    private SphereCollider sphereColl;
    private MeshRenderer meshRenderer;

    private Vector3 hitPos;
    [SerializeField] private float spawnHeight = 13f;
    [SerializeField] private List<GameObject> trails;
    [SerializeField] private float explosionDelay;
    private float constantSpeed;
    private float meteorDistance;
    private bool ableToMove;
    private GameController gameController;

    //pools
    private ObjectPooler targetPool;
    private ObjectPooler impactEffectPool;
    private ObjectPooler sliceEffectPool;

    //player
    private Transform player;
    [SerializeField] private int minDamage = 2;
    [SerializeField] private int maxDamage = 10;

    //target
    private GameObject target;
    private bool sceneStarted;
    [SerializeField] private Vector3 meteorOffSetFromTarget;
    [SerializeField] private Transform ground;

    //coins spawn
    [SerializeField] private bool spawnRandomly = true;

    //enemies
    private Enemies enemies;
    private void Awake()
    {
        sceneStarted = true;

        rb = GetComponent<Rigidbody>();
        sphereColl = GetComponent<SphereCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        player = Utils.findComponent<Transform>("Player");

        targetPool = Utils.findComponent<ObjectPooler>("Target Pool");

        impactEffectPool = Utils.findComponent<ObjectPooler>("Impact Effect Pool");

        sliceEffectPool = Utils.findComponent<ObjectPooler>("Slice Effect Pool");

        gameController = Utils.findComponent<GameController>("Game Controller");

        enemies = Utils.findComponent<Enemies>("Enemies");
    }
    void OnEnable()
    {
        //enable movement
        ableToMove = true;

        resetMeteorVelocity();

        if (!sceneStarted) hitPos = getMeteorHitPosition();

        spawnTarget(hitPos, Quaternion.identity);

        transform.position = getStartingPos();

        setMeteorRotationToTarget(hitPos);

        constantSpeed = getMeteorConstantSpeed();

        sceneStarted = false;
    }
    #region OnEnable Methods
    private void resetMeteorVelocity()
    {
        rb.velocity = Vector3.zero;
    }
    private Vector3 getMeteorHitPosition()
    {
        if (spawnRandomly)
        {
            int randomNum = Random.Range(0, 1);
            if (randomNum == 0 && gameController.coinsSpawning)
            {
                return gameController.getLastCoinPosition();
            }
            else
            {
                float maxRange = (ground.localScale.x * 5f) - (transform.GetChild(0).localScale.x / 2f);
                Vector3 meteorSpawnPos = transform.position;

                if (gameController != null && sceneStarted)
                {
                    meteorSpawnPos.x = Mathf.Clamp(gameController.getRandomSpot(), -maxRange, maxRange);
                }
                else meteorSpawnPos.x = Random.Range(-maxRange, maxRange);

                return meteorSpawnPos;
            }
        }
        else return transform.position;
    }
    private void spawnTarget(Vector3 position, Quaternion rotation)
    {
        if (targetPool != null && !sceneStarted)
            target = targetPool.getObject(position, rotation);
    }
    private Vector3 getStartingPos()
    {
        float randomX = Random.Range(-spawnHeight, spawnHeight);
        float randomY = spawnHeight;
        float zModifier = spawnHeight / 5;
        float randomZ = Random.Range(zModifier, spawnHeight);
        Vector3 offSet = new Vector3(randomX, randomY, randomZ);
        Vector3 startingPos = hitPos + offSet;
        return startingPos;
    }
    private void setMeteorRotationToTarget(Vector3 position)
    {
        transform.LookAt(position);
    }
    private float getMeteorConstantSpeed()
    {
        meteorDistance = Vector3.Distance(hitPos + meteorOffSetFromTarget, transform.position);
        if (player != null)
        {
            float playerMovementSpeed = player.GetComponent<PlayerMovementByTouch>().forwardSpeed;
            float playerDistance = Vector3.Distance(hitPos, player.position);
            return meteorDistance * playerMovementSpeed / playerDistance;
        }
        else return meteorDistance / 2;
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

    #region OnCollisionEnter Methods
    private IEnumerator trigger(Collision other)
    {
        yield return new WaitForSeconds(explosionDelay);

        ContactPoint contact = other.contacts[0];
        Vector3 pos = contact.point;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

        if (other.gameObject.tag == "Player")
        {
            int damageToTake = Random.Range(minDamage, maxDamage);
            GameObject player = other.gameObject;

            if (player.GetComponent<PlayerMovementByTouch>().isSlicing)
            {
                if (sliceEffectPool != null) sliceEffectPool.getObject(pos, rot);
                enemies.enemiesBack();
                explode();
                yield break;
            }
            else
            {
                //damaging the player and creating camera shake effect
                player.GetComponent<PlayerHealth>().takeDamage(damageToTake);
                if (useCameraShakeWhenHit && CameraShaker.Instance != null)
                    CameraShaker.Instance.ShakeOnce(8f, 8f, 0.1f, 1f);
            }
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

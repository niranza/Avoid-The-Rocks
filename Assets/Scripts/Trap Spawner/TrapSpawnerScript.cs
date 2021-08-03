using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnerScript : MonoBehaviour
{
    public TrapSpawner trapSpawner;
    public ObjectPooler triggerBoxPool;
    public ObjectPooler projectilePool;
    [System.NonSerialized] public bool spawnObject;
    [System.NonSerialized] public Vector3 projectileOffSetFromPlayer;
    private int minBlocksFromPlayer;
    private int maxBlocksFromPlayer;
    private int trapsAhead;

    private Transform player;
    private bool hasSceneStarted;
    private void Awake()
    {
        hasSceneStarted = false;
        player = findComponent<Transform>("Player");

        projectileOffSetFromPlayer = trapSpawner.offSetFromPlayer;
        minBlocksFromPlayer = trapSpawner.minBlocksBetweenTraps;
        maxBlocksFromPlayer = trapSpawner.maxBlocksBetweenTraps;
        trapsAhead = trapSpawner.trapsAhead;
        spawnObject = trapSpawner.spawnObject;
    }
    private void OnEnable()
    {
        if (hasSceneStarted)
            restartTrapSpawner(1);
        hasSceneStarted = true;
    }
    void Start()
    {
        SpawnTrapsAhead(trapsAhead);
    }
    public void spawnNewTrap()
    {
        SpawnTrigger();
        moveTrapSpawner();
    }
    private void SpawnTrigger()
    {
        TrapTrigger newTrigger = triggerBoxPool.getObject(transform.position, Quaternion.identity).
        GetComponent<TrapTrigger>();
        if (newTrigger != null)
            newTrigger.trapSpawner = this;
        if (spawnObject) projectilePool.getObject(transform.position + projectileOffSetFromPlayer, Quaternion.identity);
    }
    private void moveTrapSpawner()
    {
        float z = Random.Range(minBlocksFromPlayer, maxBlocksFromPlayer);
        transform.Translate(new Vector3(0, 0, z));
    }

    private void restartTrapSpawner(int offSet)
    {
        if (player != null)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                player.position.z + offSet
                );

            SpawnTrapsAhead(trapsAhead);
        }
    }
    private void SpawnTrapsAhead(int num)
    {
        for (int i = 0; i < num; i++) spawnNewTrap();
    }

    private T findComponent<T>(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
            return obj.GetComponent<T>();
        return default;
    }
}

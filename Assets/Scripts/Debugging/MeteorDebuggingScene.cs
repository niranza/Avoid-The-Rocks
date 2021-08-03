using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDebuggingScene : MonoBehaviour
{
    public ObjectPooler projectilePool;
    public int amountToSpawn = 1;
    public bool spawnAtTheBegging = true;
    public bool fromPool = false;
    private void Start()
    {
        if (spawnAtTheBegging) spawnProjectile(amountToSpawn, fromPool);
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) spawnProjectile(amountToSpawn, fromPool);
    }

    private void spawnProjectile(int amount, bool isFromPool)
    {
        for (int i = 0; i < amount; i++)
            projectilePool.getObject(transform.position, Quaternion.identity);
    }
}

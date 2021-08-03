using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private Transform ground;
    [System.NonSerialized] public TrapSpawnerScript trapSpawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && trapSpawner.enabled)
        {
            if (!trapSpawner.spawnObject)
                trapSpawner.projectilePool.getObject(
                    transform.position + trapSpawner.projectileOffSetFromPlayer
                    , Quaternion.identity
                    );

            trapSpawner.spawnNewTrap();

            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTrap : MonoBehaviour
{
    [SerializeField] private ObjectPooler projectilePool;
    [SerializeField] private Vector3 projectileOffSet;
    private Vector3 startingPos;
    private bool hasSpawnedProjectile;
    private MeshRenderer meshRennderer;
    private void Awake()
    {
        meshRennderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        meshRennderer.enabled = false;
    }
    private void OnEnable()
    {
        startingPos = transform.position;
        transform.position -= projectileOffSet;
        hasSpawnedProjectile = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasSpawnedProjectile)
        {
            hasSpawnedProjectile = true;
            projectilePool.getObject(startingPos, Quaternion.identity);
            StartCoroutine(waitForNextSpawn());
        }
    }
    private IEnumerator waitForNextSpawn()
    {
        yield return new WaitForSeconds(1f);
        hasSpawnedProjectile = false;
    }
}

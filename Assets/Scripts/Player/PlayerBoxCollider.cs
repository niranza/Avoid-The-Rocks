using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoxCollider : MonoBehaviour
{
    [SerializeField] private float offSet = 0f;
    [SerializeField] private BoxCollider targetCollider;
    [SerializeField] private ParticleSystem powerUpEffect;
    [SerializeField] private PlayerMovementByTouch playerMovement;
    private BoxCollider boxCollider;
    private float forwardSpeed;
    private float waitForHitTime;
    private float distance;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        forwardSpeed = playerMovement.forwardSpeed;
        waitForHitTime = playerMovement.waitForHitDuration;
    }
    private void Start()
    {
        Vector3 size = boxCollider.size;
        Vector3 center = boxCollider.center;
        distance = (forwardSpeed * waitForHitTime) + offSet;
        size.z = distance - (targetCollider.size.z / 2f);
        center.z = (size.z / 2f) + 0.5f;
        boxCollider.size = size;
        boxCollider.center = center;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target") StartCoroutine(readyToHit());
    }
    private IEnumerator readyToHit()
    {
        powerUpEffect.Play();
        //put here some animation
        yield return new WaitForSeconds(distance / forwardSpeed);
        //end the animation here
        powerUpEffect.Stop();
    }
}

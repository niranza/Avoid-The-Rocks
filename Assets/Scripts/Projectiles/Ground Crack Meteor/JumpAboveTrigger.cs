using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class JumpAboveTrigger : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            other.GetComponent<PlayerHealth>().takeDamage(damage);
            CameraShaker.Instance.ShakeOnce(2f, 20f, 0.1f, 1f);
        }
    }
}

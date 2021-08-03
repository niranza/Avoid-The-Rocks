using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoxCollider : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().takeDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEndCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            transform.parent.gameObject.SetActive(false);
    }
}

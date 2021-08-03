using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseSetterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Terrain")
        {
            other.transform.root.gameObject.SetActive(false);
        }
    }
}

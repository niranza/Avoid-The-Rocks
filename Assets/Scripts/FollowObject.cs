using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform followedObject;
    private Vector3 pos;
    private float zOffSet;
    private void Start()
    {
        pos = transform.position;
        if (followedObject != null)
            zOffSet = transform.position.z - followedObject.position.z;
    }
    void Update()
    {
        if (followedObject != null)
        {
            pos.z = followedObject.position.z + zOffSet;
            transform.position = pos;
        }
    }
}

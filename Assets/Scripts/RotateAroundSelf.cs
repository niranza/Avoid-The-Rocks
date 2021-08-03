using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSelf : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    [Header("Axis")]
    [SerializeField] private bool x = false;
    [SerializeField] private bool y = false;
    [SerializeField] private bool z = false;
    private Vector3 rotation;
    private void Start()
    {
        rotation = Vector3.zero;
    }
    void Update()
    {
        if (x) rotation.x = Time.deltaTime * speed;
        if (y) rotation.y = Time.deltaTime * speed;
        if (z) rotation.z = Time.deltaTime * speed;
        transform.Rotate(rotation);
    }
}

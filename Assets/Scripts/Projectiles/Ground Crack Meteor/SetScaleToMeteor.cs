using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScaleToMeteor : MonoBehaviour
{
    [SerializeField] private Transform largeMeteor;
    [SerializeField] private float heightOffSet;
    private Vector3 startingScale;
    private Vector3 startingPos;
    private void Start()
    {
        startingScale = new Vector3(
            largeMeteor.localScale.x,
            transform.localScale.y,
            largeMeteor.localScale.z
            );
        transform.localScale = startingScale;

        startingPos = transform.position;
        startingPos.y = (transform.localScale.y / 2f) + heightOffSet;
        transform.position = startingPos;
    }
}

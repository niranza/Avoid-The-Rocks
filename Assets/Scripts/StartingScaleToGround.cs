using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScaleToGround : MonoBehaviour
{
    [SerializeField] private Transform ground;
    private Vector3 startingScale;
    private Vector3 startingPos;
    private void Start()
    {
        startingScale = new Vector3(
            ground.localScale.x * 10,
            transform.localScale.y,
            transform.localScale.z
            );
        transform.localScale = startingScale;
        startingPos = new Vector3(
            ground.position.x,
            transform.position.y,
            transform.position.z
            );
        transform.position = startingPos;
    }
}

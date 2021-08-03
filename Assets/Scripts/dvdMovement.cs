using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dvdMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> positions;
    [SerializeField] private float speed = 1;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private float t;
    private int index;
    private float lerpTime;
    private void Start()
    {
        index = 1;
        getTargetPos();
    }

    private void Update()
    {
        lerpTime = Time.deltaTime * speed;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpTime);
        t = Mathf.Lerp(t, 1f, lerpTime);
        if (t > 0.7f) getTargetPos();
    }
    private void getTargetPos()
    {
        t = 0;
        if (index == positions.Count) index = 0;
        targetPos = positions[index].position;
        targetRot = positions[index].rotation;
        index++;
    }
}

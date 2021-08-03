using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    //movement
    [SerializeField] [Range(0f, 4f)] private float lerpTime = 1;
    [SerializeField] private Transform gfx;
    [SerializeField] private Transform ground;
    [SerializeField] private float distanceFromFrame = 5f;
    private Vector3 hitPos;
    private Vector3 startingPos;
    private Vector3 destination;

    //Trigger
    [SerializeField] private CapsuleCollider playerColl;
    [SerializeField] private BoxCollider boxColl;
    [SerializeField] private BoxCollider endBoxColl;
    private void OnEnable()
    {
        //x
        float x = ground.localScale.x * 5;

        //resizing box colliders with the ground
        resetBoxColliderSize(x);
        resetEndBoxColliderSize(x);

        //creating a position to start at
        hitPos = gfx.position;
        Vector3 offSet = new Vector3(x + distanceFromFrame, 0, 0);
        startingPos = hitPos + offSet;
        gfx.position = startingPos;

        //creating a position for the alien to reach
        destination = hitPos;
        destination.x = x;
    }

    private void Update()
    {
        gfx.position = Vector3.Lerp(gfx.position, destination, Time.deltaTime * lerpTime);
    }
    private void resetBoxColliderSize(float x)
    {
        float colliderOffSet = (playerColl.radius * 2) + 1;
        Vector3 newCollSize = new Vector3(x * 2, 10, boxColl.size.z);
        boxColl.size = newCollSize;
        Vector3 newCollCenter = new Vector3(boxColl.center.x, (newCollSize.y / 2f) + colliderOffSet, boxColl.center.z);
        boxColl.center = newCollCenter;
    }
    private void resetEndBoxColliderSize(float x)
    {
        Vector3 newCollSize = new Vector3(x * 2, 10, boxColl.size.z);
        endBoxColl.size = newCollSize;
        Vector3 newCollCenter = new Vector3(boxColl.center.x, newCollSize.y / 2f, boxColl.center.z);
        endBoxColl.center = newCollCenter;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDebug : MonoBehaviour
{
    Vector3 pos;
    float timer;
    private void OnEnable()
    {
        pos = transform.position;
        timer = 0;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (pos != transform.position)
        {
            Debug.LogError(
                    "the target is not in the right position at: " + transform.position +
                    "\nthe target suppose to be at position " + pos
                     + "\nMeteor"
                    );
        }
    }
    private void OnDisable()
    {
        float framesSurvived = timer / Time.deltaTime;
        if (timer < 0.1f) Debug.LogError("Target died too quick at: " + transform.position + "\nSurvived " + framesSurvived + " frames");
    }
}

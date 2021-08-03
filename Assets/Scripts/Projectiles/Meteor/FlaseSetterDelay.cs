using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaseSetterDelay : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    private float timer;
    private void OnEnable()
    {
        timer = 0f;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }
    }
}

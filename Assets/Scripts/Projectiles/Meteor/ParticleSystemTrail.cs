using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTrail : MonoBehaviour
{
    public float delay = 1f;
    private ParticleSystem ps;
    [System.NonSerialized]
    [SerializeField] private Transform parent;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        parent = transform.parent;
    }
    private void OnEnable()
    {
        transform.position = parent.position;
    }
    public void setFalseWithDelay()
    {
        StartCoroutine(destroyGameObject());
    }
    private IEnumerator destroyGameObject()
    {
        if (ps != null)
        {
            delay = ps.main.duration + ps.main.startLifetime.constantMax;
            transform.parent = null;
            if (ps.isPlaying) ps.Stop();
        }
        yield return new WaitForSeconds(delay);

        if (ps != null)
        {
            transform.SetParent(parent);
        }
    }
}

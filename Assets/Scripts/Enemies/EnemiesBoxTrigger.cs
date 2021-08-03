using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBoxTrigger : MonoBehaviour
{
    [SerializeField] private Transform enemies;
    [SerializeField] private bool killPlayer = true;
    private Enemies enemiesScript;
    private void Awake()
    {
        enemiesScript = enemies.GetComponent<Enemies>();
    }
    private void Update()
    {
        transform.position = enemies.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemiesScript.setAbleToMove(false);
            if (killPlayer)
                other.GetComponent<PlayerHealth>().die();
        }
    }
}

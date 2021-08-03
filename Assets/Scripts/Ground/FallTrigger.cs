using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        GameObject gameManagerObj = GameObject.Find("Game Manager");
        if (gameManagerObj != null)
            gameManager = gameManagerObj.GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playPlayerFallingAnimation(other);
            if (gameManager != null) gameManager.restart();
            StartCoroutine(addForce(other));
        }
    }
    private IEnumerator addForce(Collider other)
    {
        yield return new WaitForSeconds(0.1f);
        other.GetComponent<Rigidbody>().AddForce(0f, -25000f, 0f);
    }
    private void playPlayerFallingAnimation(Collider other)
    {
        Animator anim = other.GetComponentInChildren<Animator>();
        if(anim != null)
        {
            anim.SetBool("jump_up", true);
            anim.SetBool("jump_down", false);
        }
    }
}

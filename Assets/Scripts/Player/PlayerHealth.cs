using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Enemies enemiesScript;
    public int maxHealth = 31;
    private int health = 10;

    //for death
    private Rigidbody rb;
    private PlayerMovementByTouch playerMovement;
    private GameManager gameManager;

    private void Awake()
    {
        GameObject gameTimerObj = GameObject.Find("Game Manager");
        if (gameTimerObj != null)
            gameManager = gameTimerObj.GetComponent<GameManager>();

        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovementByTouch>();
    }
    private void Start()
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }
    public void takeDamage(int damage)
    {
        health -= damage;
        healthBar.setHealth(health);
        enemiesScript.moveToMidPoint(transform.position);

        //show the damage in the U.I
        if (health <= 0)
        {
            die();
        }
    }
    public void die()
    {
        playerMovement.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        StartCoroutine(addForce());
        gameManager.restart();
    }
    private IEnumerator addForce()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(0, 100, 50000);
    }
}

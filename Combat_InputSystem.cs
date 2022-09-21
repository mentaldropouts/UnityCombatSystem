using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat_InputSystem : MonoBehaviour
{
    [Header("Player Components")]
    Animator animator;
    PlayerInputActions input;
    CharacterController controller;

    [Header("Checks for combat")]
    private bool enemyInRange = false;
    private float playerHealthCur;
    private bool attackPressed = false;
    private bool blockedPressed = false;
    private bool dodgePressed = false;

    [SerializeField] private GameObject enemyMain;
    [SerializeField] private GameObject enemyChild; 
    [SerializeField] private EnemyHealth enemyHealth;

    [SerializeField] private float attackPoints = 2;
    [SerializeField] private float playerHealthMax = 10;



void Awake()
    {
        playerHealthCur = playerHealthMax;
        input = new PlayerInputActions();
    }

private void OnTriggerEnter2D(Collider2D other){
        //if (GetComponent<Collider>().gameObject.tag == "Enemy")
    if(other.gameObject.tag == "Enemy"){
        //Gets main enemy body
        
            enemyMain = other.gameObject;
            enemyChild = other.gameObject.transform.Find("Health").gameObject;
            enemyHealth = enemyChild.GetComponent<EnemyHealth>();
            enemyInRange = true;

    Debug.Log("Enemy is in collider");


        }        }

private void OnTriggerExit2D(Collider2D collider){   
        if (collider.gameObject.tag == "Enemy")
        {
            enemyInRange = false;
        }
    }

public void attack()
{
    if(attackPressed && enemyInRange)
    {
        Debug.Log("ATTACK INITIATED");
        //float enemyHealth = Enemy.getHealth() - attackPoints;
        float eHealth = enemyHealth.getHealth();
        eHealth = eHealth - attackPoints;
        if(eHealth < 0)
        {
        eHealth = 0;
        }

        enemyHealth.setHealth(eHealth);
        if(eHealth == 0)
        {
        Destroy(enemyMain);
        }

        }

    }

//-----------------
//ANIMATION SCRIPTS
//----------------











}

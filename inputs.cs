using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class inputs : MonoBehaviour
{
    [SerializeField] private GameObject playerMain;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float attackPoints = 2.0f;

    private PlayerInput input;
    private PlayerInputActions playerInputActions;
    private float smoothInputSpeed;
    private bool movePressed; 
    private bool attackPressed;
    public bool blockPressed;
    private bool enemyInRange;
    private Vector2 currentMovement;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;


    [Header("Enemy in Collider")]
    [SerializeField] private GameObject enemyMain;
    [SerializeField] private GameObject enemyChild; 
    [SerializeField] private EnemyHealth enemyHealth;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerMain = GameObject.FindGameObjectWithTag("Player");
        controller = playerMain.GetComponent<CharacterController>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Actions.Enable();
        playerInputActions.Actions.Movement.performed += Movement_Performed;
        playerInputActions.Actions.Attack.performed += Attack_Performed;
        playerInputActions.Actions.Block.performed += Block_Performed;

    }

    private void FixedUpdate()
    {
        //So that it is called more than once a frame
        moveCharacter();
    }

public void Movement_Performed(InputAction.CallbackContext context)
{

    //Purpose: to utilize the context and move the character accordingly
    if(movePressed)
    {
    //Debug.Log(context);
    currentMovement = context.ReadValue<Vector2>();
   // Debug.Log("CurrentMove " + currentMovement);
    movePressed = currentMovement.x != 0 || currentMovement.y !=0;
    //Smooths out the distance so that the movement seems more real
   //Debug.Log(currentMovement);
    currentInputVector = Vector2.SmoothDamp(currentInputVector,currentMovement, ref smoothInputVelocity, smoothInputSpeed);
    //Debug.Log("Movement! " + context.phase);
    }
    else
    {
        currentMovement = new Vector2(0.0f,0.0f);
    }
}

public void GetMovePressed(InputAction.CallbackContext context)
{
    if(context.performed)
    {
        movePressed = true;
    }
    else 
    {
        movePressed = false;
    }
}

private void moveCharacter()
{
    if(movePressed)
    {
    Vector3 move = new Vector2(currentInputVector.x, currentInputVector.y);
   // Debug.Log("CurrentInputVector.x " + currentInputVector.x);
    //Debug.Log("CurrentInputVector.y " + currentInputVector.y);
    controller.Move(move * Time.deltaTime * speed);
    }
    else
    {
        Vector3 move = new Vector2(0.0f,0.0f);
        controller.Move(move * Time.deltaTime * speed);
    }
}


//ACTION FUNCTIONS

public void GetAttackPressed(InputAction.CallbackContext context)
{
    if(context.performed)
    {
        attackPressed = true;
    }
    else
    {
        attackPressed = false;
    }
}


public void Attack_Performed(InputAction.CallbackContext context)
{
    Debug.Log("ATTACK INITIATED");
    if(attackPressed && enemyInRange)
    {  
        float eHealth = enemyHealth.getHealth();
        eHealth = eHealth - attackPoints;
        if(eHealth < 0) 
        {
            eHealth = 0;
        }
        else   
        {
        enemyHealth.setHealth(eHealth);
        }

        if(eHealth == 0)
        {
            //Destrys the entire enemy 
            Destroy(enemyMain);
        }
    }
}

private void OnTriggerEnter2D(Collider2D other)
{   
    Debug.Log("There is a collider entering the player's collider");
    //if (GetComponent<Collider>().gameObject.tag == "Enemy")
    if(other.gameObject.tag == "Enemy"){
        //Gets main enemy body
        enemyMain = other.gameObject;
        enemyChild = other.gameObject.transform.Find("Health").gameObject;
        enemyHealth = enemyChild.GetComponent<EnemyHealth>();
        enemyInRange = true;
        //Debug.Log("Enemy is in collider");
    }
}

private void OnTriggerExit2D(Collider2D collider)
{   
        if (collider.gameObject.tag == "Enemy")
        {
            enemyInRange = false;
        }
}

public void Block_Performed(InputAction.CallbackContext context)
{
if(blockPressed)
{
Debug.Log("Block was performed");
}

}

public void GetBlockPressed(InputAction.CallbackContext context)
{
    //for playerInputActions to get the bool of the keys being pressed
    if(context.performed)
    {
        blockPressed = true;
    }
    else 
    {
        blockPressed = false;
    }
    //Debug.Log(blockPressed);
}

public bool blocking()
{
   // Debug.Log("block = " + blockPressed);
    return blockPressed;
}

}

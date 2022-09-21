using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    private bool movePressed = false;
    private Vector2 moveDirection;
    private bool interactPress;
    private bool jumpPressed;
    private bool dashPressed;
    private bool attackPressed;
    private static inputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }

    public static inputManager GetInstance() 
    {
        return instance;
    }

    private void Update(){
        
    }

    private void FixedUpdate(){

    }

    public void jPressed(InputAction.CallbackContext context){
        if(context.performed){
            jumpPressed = true;
        }
        else{
            jumpPressed = false;
        }
    }

    public void dPressed(InputAction.CallbackContext context){
        if(context.performed){
            dashPressed = true;
        }
        else{
            dashPressed = false;
        }
      Debug.Log(dashPressed);
    }

     public bool interact(InputAction.CallbackContext context){
        if(context.performed){
            interactPress = true;
        }
        else{
            interactPress = false;
        }
        return interactPress;
    }

    public void movePress(InputAction.CallbackContext context){
        if(context.performed){
            movePressed = true;
        }
        else{
            movePressed = false; 
        }
    }

    public void attackPress(InputAction.CallbackContext context){
    if(context.performed){
        attackPressed = true;
    }
    else{
        attackPressed = false;
    }
    Debug.Log(attackPressed);
    }
     public bool GetInteractPressed(){
        bool result = interactPress;
        interactPress = false;
        return result;
    }

    public bool GetJumpPressed(){
        bool result = jumpPressed;
        //jumpPressed = false;
        return result;
    }

    public bool GetDashPressed()
    {   
        bool result = dashPressed;
        //Debug.Log(dashPressed);
        return result;
    }

    public bool GetMovePressed(){
        bool result = movePressed;
        return result;
    }

}
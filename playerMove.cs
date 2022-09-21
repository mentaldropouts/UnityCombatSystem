using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    [Header("Player Components")]
    //InputAction move;
    Animator animator;
    public PlayerInputActions input;
    public CharacterController controller;

    [Header("Specifications")]
    [SerializeField] public float Gravity  = 10.0f;
    [SerializeField] public float speed = 10f;
    [SerializeField] public float gravityScale = 5;
    [SerializeField] public float jumpForce = 5.0f;
    [SerializeField] public float smoothInputSpeed = 0.2f;
    [SerializeField] public float dashSpeed = 20.0f;
    [SerializeField] public float dashMult = 2;
    [SerializeField] public float dashDuration = 1.0f;
    private float jumpHeight = 10;
    public float maxFallSpeed = 100f;

    [Header("States")]
    public int isWalkingHash;
    public int isJumpingHash;
    public int isDashingHash;

    [Header("Bool States")]
    bool isWalking;
    bool isDashing;
    bool canDash = true;

    [Header("Movements")]
    int currentJump;
    Vector2 currentMovement;
    Vector2 nextMovement;
    Vector2 currentInputVector;
    Vector2 currentJumpVector;
    private float verticalVelocity;
    private float groundMath;
    private Vector3 verticalMovement;
    private Vector3 peakVel;
    [SerializeField] public Vector3 smoothInputVelocity;

    [Header("Checks")]
    bool movePressed;
    bool jumpPressed;
    bool dashPressed;
    bool groundedPlayer;

    private void Awake(){
        controller = GetComponent<CharacterController>();
        //Gets the player input from the player object
        input = new PlayerInputActions();
    }

    void Update(){
    if(groundedPlayer) {
          canDash = true;
    }
    movePressed = inputManager.GetInstance().GetMovePressed();
    jumpPressed = inputManager.GetInstance().GetJumpPressed();
    handleMovement();
    handleJump();
    }

    void FixedUpdate(){
        groundedPlayer = controller.isGrounded;
        if(!groundedPlayer){
            Ground();
        }
        movement();
        dash();
        jump();
    }

    void Start(){
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
    
    }

     private void Ground(){
        //Purpose: To check if the controller is grounded and if not then move controller to the ground
        if(!groundedPlayer){
        groundMath = Mathf.Clamp(groundMath - Gravity * Time.deltaTime, -maxFallSpeed, Mathf.Infinity);
        controller.Move(new Vector3(0, groundMath, 0) * Time.deltaTime);
        }
    }

    public void movement(){
        //Debug.Log("Movement was called");
            if(movePressed){
               Debug.Log("Movement was pressed");
            //inputs the Vector2 values inputted by the Player Input
            input.Actions.Movement.performed += context =>{
                currentMovement = context.ReadValue<Vector2>();
                Debug.Log(currentMovement.x);
                movePressed = currentMovement.x != 0 || currentMovement.y !=0;
                Debug.Log(movePressed);
            };
            //Smooths out the distance so that the movement seems more real.
            currentInputVector = Vector3.SmoothDamp(currentInputVector,currentMovement, ref smoothInputVelocity, smoothInputSpeed);
            Vector3 move = new Vector3(currentInputVector.x, 0, currentMovement.y);
           // Debug.Log(currentMovement);
            controller.Move(move * Time.deltaTime * speed);
        }
    }

    public void dash(){
        if(canDash && dashPressed){
            input.Actions.Dash.performed += context => {
                //Take the values from movement and multipliy them
                Debug.Log("Dash Pressed");
            };
        }
    }

    private void jump(){
        verticalVelocity = -Gravity * Time.deltaTime;
            //Player Jump and Player Jump Moving
            if(groundedPlayer){verticalMovement.y = 0.0f;}
            if(jumpPressed && groundedPlayer){
                verticalMovement.y += Mathf.Sqrt(jumpForce * -2.0f * -(Gravity));
                peakVel = new Vector3(currentMovement.y, verticalMovement.y, 0.0f);
                peakVel.y += -(Gravity) * Time.deltaTime;
                controller.Move(peakVel * Time.deltaTime);
            }
        }
        //-------------------

        //ANIMATION FUNCTIONS//

        //-------------------//
    private void handleJump(){
        bool isJumping = animator.GetBool(isJumpingHash);
        if(jumpPressed && !isJumping){
            animator.SetBool(isJumpingHash,true);
        }

        if(!jumpPressed && isJumping && groundedPlayer){
            animator.SetBool(isJumpingHash,false);
        }
    }

   

     private void handleMovement(){
        //Current state
        bool isWalking = animator.GetBool(isWalkingHash);
     }

}

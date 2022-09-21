using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Written by Taylor Payne during the Summer of 2022
//Multi-Purpose Ai controller that handles combat as well as path finding the player
//Pre-Cond = PlayerPrefab that has a child of "Health"

public class AiController : MonoBehaviour
{

[SerializeField] public float moveSpeed = 0.001f;
Animator animator;
//private var pathChoice; //Generates random number to determine which path the Ai will take 

[Header("Game Objects")]
private GameObject player;
private GameObject playHealthObj;
private PlayerHealth playerHealth;

private GameObject parent;
private GameObject[] AIs; //incase the AI needs to know about other AI like if it gets scared and runs away  

[Header ("Positions")]
private Vector3 playerPos;
private Vector3 AIPosition;
private float distanceX;
private float distanceY;
private float totalDist;
private int pathChoice;

[Header("Boolean Positions")]
private bool isAbove;
private bool isRight;
private bool xEquiv; //if they are on the same X Coordinate;
private bool yEquiv; //if they are on the same Y Coordinate;


[Header("Hash States")]
public int idleHash;
public int seekingHash;
public int stanceHash;  //once the Ai finds the player
public int attackHash;

[Header("Bools")]
private bool isIdle;
private bool isSeeking;
private bool isAttacking;
private bool isWaiting; //if there are already Ai fighting player
private bool canAttack = true;


public bool mUp = false;
public bool mDown = false;
public bool mHoriz = false;
public bool mWait = false;
public bool mSeek = false;
public bool mAttack = false;

private bool playerInRange;
private float waitTime;
private float waitRange =  3.0f;   //min Range
private float noticeRange = 10.0f; //max range 
[SerializeField] private float attackPoints;
private float time = 0.0f;
private float coolDown = 1.0f;
[SerializeField] private float fireRate = 0.1f;

void Start()
{
    parent = this.gameObject;
    pathChoice = Random.Range(1,4);
    animator = GetComponent<Animator>();

    idleHash = Animator.StringToHash("isIdle");
    seekingHash = Animator.StringToHash("isSeeking");
    stanceHash = Animator.StringToHash("isWaiting");
    attackHash = Animator.StringToHash("isAttacking");
    
    player = GameObject.FindGameObjectWithTag("Player");
    playHealthObj = player.gameObject.transform.Find("Health").gameObject; 
    playerHealth = playHealthObj.GetComponent<PlayerHealth>(); 
  }

void Update()
{
//Get and store the players position so we can move the Ai towards it
playerPos = player.transform.position;
AIPosition = this.transform.position;

//Path finding functions
ComparingPos();
choosingPath();
getDistance();

//Attack functions
PlayerInRange();
}


void ComparingPos()
{
    if(playerPos.x > AIPosition.x)
    {
        //Debug.Log("Player is to the Right");
        isRight = true;
        xEquiv = false;
    }
    else if(playerPos.x < AIPosition.x)
    {
        //Debug.Log("Player is to the Left");
        isRight = false;
        xEquiv = false;
    }
    else
    {
        xEquiv = true;
    }
   // Debug.Log(xEquiv);

    if(playerPos.y > AIPosition.y)
    {
        //Debug.Log("Player is Above");
        isAbove = true;
        yEquiv = false;
    }
    else if(playerPos.y < AIPosition.y)
    {
        //Debug.Log("Player is to the Left");
        isAbove = false;
        yEquiv = false;
    }
    else
    {
        yEquiv = true;
    }
}

void choosingPath()
{
  int pathChoice = Random.Range(1,4);
    //To determine the range where the enemy will follow player
    if(totalDist > waitRange && totalDist < noticeRange){

    //Seeks to make the Y axis equivalent
        if(pathChoice == 1 )
        {
            if(!yEquiv)
            {
                if(isAbove)
                {
                    transform.position += new Vector3 (0,moveSpeed);
                }
                else
                {
                transform.position += new Vector3 (0,-moveSpeed);
                }
            }
            else 
            {
                if(!xEquiv)
                {
                    if(isRight)
                    {
                        transform.position += new  Vector3 (moveSpeed,0);
                    }
                    else
                    {
                    transform.position += new Vector3(-moveSpeed,0);
                    }
                }

            }
        }


//Seeks a direct path to the target
  if(pathChoice == 2)
  {
    if(isRight && isAbove)
    {
        transform.position += new  Vector3 (moveSpeed,moveSpeed);
    }
    else if(isRight && !isAbove)
    {
        transform.position += new  Vector3 (moveSpeed,-moveSpeed);
    }
    else if(!isRight && isAbove)
    {
        transform.position += new  Vector3  (-moveSpeed,moveSpeed);
    }
    else 
    {
        transform.position += new  Vector3 (-moveSpeed,moveSpeed);
    }
  }

//Seeks to align the X axis 
if(pathChoice == 3)
{
if(!xEquiv)
    {
        if(isRight)
        {
            transform.position += new Vector3 (moveSpeed,0);
        }
        else
        {
            transform.position += new Vector3 (-moveSpeed,0);
        }
    }
    else 
    {
        if(!yEquiv)
        {
            if(isAbove)
            {
                transform.position += new Vector3(0,moveSpeed);
            }
            else
            {
                transform.position += new Vector3(0,moveSpeed);
            }
        }
    }
}

}
}

void getDistance()
{

//Getting the distances of both the x and y from the AI and the Player
if(isRight)
{
    distanceX = playerPos.x - AIPosition.x;
}
else 
{
    distanceX = AIPosition.x - playerPos.x;
}

if(isAbove)
{
    distanceY = playerPos.y - AIPosition.y;
}
else 
{
    distanceY = AIPosition.y - playerPos.y;
}

//Converting the distances into the hypotenuse of a triangle

totalDist = Mathf.Sqrt(Mathf.Pow(2,distanceX) + Mathf.Pow(2,distanceY));
//Debug.Log(totalDist);
}

private void PlayerInRange()
{
    //player is in past or at wait range
    if(totalDist <= waitRange)
    {
        playerInRange = true;
        //if the coolDown is small
        if(coolDown < Time.time)
        {
            playerHealth.enemyAttack(attackPoints);
            coolDown = Time.time + fireRate;
        }
    }
    else
    {
        playerInRange = false;
    }
}

 void OnDrawGizmos()
     {
        //Draws a blue line to the player
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(AIPosition, playerPos);
    }


}
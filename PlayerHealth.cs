using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1000;
    [SerializeField] private float currentHealth;

    private GameObject player;
    private inputs playerInputs;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInputs = player.GetComponent<inputs>();
        currentHealth = maxHealth;
    }

    public void setHealth(float newHealth)
    {
      //  Debug.Log("setHealth was called");
        currentHealth = newHealth;
    }

    public float getHealth()
    {
        return currentHealth;
    }  

    public void enemyAttack(float attackPoints)
    {
        if(!playerInputs.blocking())
        {
            float newHealth = currentHealth - attackPoints;
            Debug.Log("Player did not block and was dealt " + attackPoints);
            setHealth(newHealth);
        }
        else
        {
            float blockedAttackPoints = attackPoints / 2;
            float newHealth = currentHealth - blockedAttackPoints;
            Debug.Log("Player has blocked and has been dealt " + blockedAttackPoints);
            setHealth(newHealth);
        }

        
        if(currentHealth <= 0)
        {  
            Destroy(player);
        }
    }


}

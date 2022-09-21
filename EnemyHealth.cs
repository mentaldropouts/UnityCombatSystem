using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float currentHealth;

    private GameObject player;
    private GameObject playerHealth;    
    private void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void setHealth(float newHealth)
    {
        Debug.Log("setHealth was called");
        currentHealth = newHealth;
    }

    public float getHealth()
    {
        return currentHealth;
    }
}

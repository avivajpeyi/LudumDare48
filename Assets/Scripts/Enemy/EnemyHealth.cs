using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class EnemyHealth : MonoBehaviour
{
    Transform player;
    // TODO: Get rid of references to ThirdPersonHealth, ThirdPersonDash
    private ThirdPersonDash playerDash;
    private ThirdPersonHealth playerHealth;
    NavMeshAgent nav;
    public bool isDead=false;

    public ParticleSystem DeathfX;
    
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            playerHealth = player.GetComponent<ThirdPersonHealth>();
            playerDash = player.GetComponent<ThirdPersonDash>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            if (playerDash.isDashing)
                Die();
            else
            {
                playerHealth.TakeDamage(20.0f);
            }
        }
    }

    void Die()
    {
        Debug.Log(name + " has died ");
        isDead = true;
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
//        Destroy(gameObject);
        Instantiate(DeathfX, transform.position, Quaternion.identity);
    }
}
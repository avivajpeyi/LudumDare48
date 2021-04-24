using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class EnemyHealth : MonoBehaviour
{
    Transform player;
    private ThirdPersonDash playerDash;
    private ThirdPersonHealth playerHealth;
    NavMeshAgent nav;

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
        Debug.Log(name + " triggered OnTriggerEnter!");
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
        Destroy(gameObject);
    }
}
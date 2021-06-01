using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class EnemyHealth : MonoBehaviour
{
    Transform player;
    private PlayerController playerController;
    NavMeshAgent nav;
    public bool isDead=false;

    public ParticleSystem DeathfX;
    
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            if (playerController.isDashing)
                Die();
            else
            {
                playerController.TakeDamage(20.0f);
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour
{
    public float maxLifetime = 7.5f;
    public float speed = 5;
    public float damageAmount = 20;
    public bool aimAtPlayer = true;
    public bool isHoming = false;

    public ParticleSystem bulletFx;

    GameObject player;
    // TODO: Get rid of references to ThirdPersonHealth, ThirdPersonDash 
    private PlayerController playerController;
    Vector3 target;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        if (aimAtPlayer)
            target = player.transform.position;
        else
            target = transform.root.position + transform.root.forward * 100;

        StartCoroutine(BeginDecay());
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoming)
        {    
            if (!playerController.isDead)
                target = player.transform.position;
        }

        this.transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime);

        if (transform.position == target)
            DestroyProjectile();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerController.isDashing)
            {
                playerController.TakeDamage(damageAmount);
            }

            DestroyProjectile();
        }
        else if (other.CompareTag("Obstacle"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Instantiate(bulletFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator BeginDecay()
    {
        yield return new WaitForSeconds(maxLifetime);
        DestroyProjectile();
    }
}
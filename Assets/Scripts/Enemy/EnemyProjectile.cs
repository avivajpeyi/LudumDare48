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
    private ThirdPersonHealth plyaerHealth;
    private ThirdPersonDash playerDash;
    Vector3 target;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            plyaerHealth = player.GetComponent<ThirdPersonHealth>();
            playerDash = player.GetComponent<ThirdPersonDash>();
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
            target = player.transform.position;

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
            if (!playerDash.isDashing)
            {
                plyaerHealth.TakeDamage(damageAmount);
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
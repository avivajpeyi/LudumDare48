using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    float timeSinceLastShot;
    public float shotEveryNseconds = 2;
    public GameObject projectile;
    private EnemySight mySight;

    public bool shootOnlyIfEnemyVisible = true;

    
    Transform player;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (player != null)
            Gizmos.DrawLine(transform.position, player.transform.position);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mySight = GetComponent<EnemySight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootOnlyIfEnemyVisible)
        {            
            if (mySight.playerVisible)
                Shoot();
        }
        else
        {
            Shoot();
        }


    }


    void Shoot()
    {
        if (timeSinceLastShot <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeSinceLastShot = shotEveryNseconds;
        }
        else
        {
            timeSinceLastShot -= Time.deltaTime;
        }
    }
}
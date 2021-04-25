using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    float timeSinceLastShot;
    public float shotEveryNseconds = 2;
    public GameObject projectile;
    private EnemySight mySight;
    private EnemyHealth enemyHealth;
    public bool shootOnlyIfEnemyVisible = true;

    private PauseHandler myPause;

    void Start()
    {
        mySight = GetComponent<EnemySight>();
        myPause = GetComponent<PauseHandler>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPause.isPaused && !enemyHealth.isDead)
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
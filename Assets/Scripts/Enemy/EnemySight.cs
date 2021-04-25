using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public LayerMask obstacleMask;

    public bool playerVisible;

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
    }

    // Update is called once per frame
    void Update()
    {
        playerVisible = PlayerIsVisible();
    }

    bool PlayerIsVisible()
    {
        Transform target = player.transform;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float dstToTarget = Vector3.Distance(transform.position, target.position);
        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
        {
            return true;
        }

        return false;
    }
}
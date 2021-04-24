using UnityEngine;
using System.Collections;
using UnityEngine.AI;


enum EnemyMovementStates
{
    TooClose,
    JustRight,
    TooFar
}

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollow : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;

    public float stoppingDist = 5.0f;
    public float retreatDist = 0.0f;

    [SerializeField] private EnemyMovementStates currentState;

    
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(r:10, 0, 0 , 0.5f);
        Gizmos.DrawSphere(this.transform.position, stoppingDist);
        Gizmos.color = new Color(r:00, 10, 0 , 0.5f);
        Gizmos.DrawSphere(this.transform.position, retreatDist);
    }

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }
    
    

    void UpdateState()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist > stoppingDist)
        {
            // Too far! move closer
            currentState = EnemyMovementStates.TooFar;
        }
        else if (dist < stoppingDist && dist  > retreatDist)
        {
            // Just right
            nav.ResetPath();
            currentState = EnemyMovementStates.JustRight;
        }
        else if (dist < retreatDist)
        {
            // Too close! run back
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            nav.SetDestination(newPos);
            currentState = EnemyMovementStates.TooClose;
        }
    }

    void Update()
    {
        UpdateState();
        AdjustMovement();
        CorrectDistances();

    }


    void CorrectDistances()
    {
        if (retreatDist > stoppingDist)
            stoppingDist = retreatDist + 2;

    }

    void AdjustMovement()
    {
        if (currentState == EnemyMovementStates.JustRight)
        {
            nav.ResetPath();
        }
        else if (currentState == EnemyMovementStates.TooFar)
        {
            nav.SetDestination(player.position);
        }
        else if (currentState == EnemyMovementStates.TooClose)
        {
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            nav.SetDestination(newPos);
        }
    }
}
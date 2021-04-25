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
    private EnemySight mySight;
    private PauseHandler myPause;

    private EnemyHealth enemyHealth;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(r: 10, 0, 0, 0.5f);
        Gizmos.DrawSphere(this.transform.position, stoppingDist);
        Gizmos.color = new Color(r: 00, 10, 0, 0.5f);
        Gizmos.DrawSphere(this.transform.position, retreatDist);
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        mySight = GetComponent<EnemySight>();
        myPause = GetComponent<PauseHandler>();
        enemyHealth = GetComponent<EnemyHealth>();
    }


    void UpdateState()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        currentState = EnemyMovementStates.JustRight;
        if (mySight.playerVisible && !enemyHealth.isDead)
        {
            if (dist > stoppingDist)
            {
                currentState = EnemyMovementStates.TooFar;
            }
            else if (dist < stoppingDist && dist > retreatDist)
            {
                currentState = EnemyMovementStates.JustRight;
            }
            else if (dist < retreatDist)
            {
                currentState = EnemyMovementStates.TooClose;
            }
        }
    }

    void Update()
    {
        if (!myPause.isPaused && player != null)
        {
            UpdateState();
            AdjustMovement();
            CorrectDistances();
        }
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
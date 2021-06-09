using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// Floor master
/// ------------
/// * Places obstacles, enemies, players
/// * Keeps track of remaining enemies/player
/// </summary>
[RequireComponent(typeof(NavMeshSurface))]
public class FloorMaster : MonoBehaviour
{
    public GameObject[] PremadeFloorContentList;
    public int indexOfEasy;

    private NavMeshSurface nav;

    private GameObject floorContent;
    [SerializeField] private List<GameObject> enemies;
    public GameObject playerSpawnPoint;

    [SerializeField] private int contentIndex;

    public bool allEnemiesDead;

    public void Update()
    {
        allEnemiesDead = AreAllEnemiesDead();
    }


    public void SpawnFloorContent(int floorCount)
    {
        if (floorCount==0)
            contentIndex = 0;
        else if (floorCount < 5)
            contentIndex = Random.Range(1, indexOfEasy);
        else
            contentIndex = Random.Range(1, PremadeFloorContentList.Length);

    
        
        floorContent = Instantiate(
            PremadeFloorContentList[contentIndex],
            transform.position,
            Quaternion.identity,
            transform.root
        );

        foreach (Transform child in floorContent.transform)
        {
            if (child.CompareTag("Enemy"))
                enemies.Add(child.gameObject);
            if (child.CompareTag("PlayerSpawn"))
            {
                playerSpawnPoint = child.gameObject;
                playerSpawnPoint.GetComponent<Renderer>().enabled = false;
            }
        }
        nav = GetComponent<NavMeshSurface>();
        nav.BuildNavMesh();
    }


    public void PauseEnemies(bool pauseSetting)
    {
        foreach (GameObject e in enemies)
        {
            e.GetComponent<PauseHandler>().isPaused = pauseSetting;
        }
    }
    
    
    public bool AreAllEnemiesDead()
    {
        
        foreach (GameObject e in enemies)
        {
            if (!e.GetComponent<EnemyHealth>().isDead)
                return false;
        }

        return true;
    }
    
}
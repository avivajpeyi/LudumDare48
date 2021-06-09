using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FloorTester : MonoBehaviour
{
    private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.FindWithTag("PlayerSpawn");
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = go.transform.position;
        Destroy(go);


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            e.GetComponent<PauseHandler>().isPaused = false;
        }

        player.GetComponent<PauseHandler>().isPaused = false;


        NavMeshSurface nav = FindObjectOfType<NavMeshSurface>();
        nav.BuildNavMesh();
        //Debug.Log("Test setup complete");
    }
}
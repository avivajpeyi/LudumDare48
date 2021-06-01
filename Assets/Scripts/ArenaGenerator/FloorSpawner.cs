using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject FloorPrefab;

    public Transform mainFloorT;
    public Transform nextFloorT;

    public float floorChangeDuration = 2;

    Vector3 mainFlr;
    Vector3 lowerFlr;
    Vector3 upperFlr;


    public GameObject Player;

    public GameObject currentFloor;
    private GameObject nextFloor;
    private GameObject oldFloor;

    public bool testing;

    public bool floorsMoving = false;


    [SerializeField] public int floorCount = 0;

    void OnDrawGizmos()
    {
        if (mainFloorT != null)
        {
            Vector3 mainFlrP = mainFloorT.position;
            Vector3 lowerFlrP = nextFloorT.position;
            Vector3 displacement = mainFlrP - lowerFlrP;
            Vector3 upperFlrP = mainFlrP + displacement;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(upperFlrP, mainFlrP);
            Gizmos.DrawSphere(upperFlrP, 1);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(lowerFlrP, mainFlrP);
            Gizmos.DrawSphere(mainFlrP, 1);
            Gizmos.DrawSphere(lowerFlrP, 1);
        }
    }


    void Start()
    {
        mainFlr = mainFloorT.position;
        lowerFlr = nextFloorT.position;
        Vector3 displacement = mainFlr - lowerFlr;
        upperFlr = mainFlr + displacement;
        currentFloor = InstantiateFloor(mainFlr);
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PauseHandler>().isPaused = false;
    }

    public void GenerateNextFloor()
    {
        if (Player != null && !floorsMoving)
            StartCoroutine(NewFloorSequence());
    }


    IEnumerator LerpPosition(Transform trans, Vector3 targetPosition, float duration)
    {
        Debug.Log(trans.name + " is lerping!");
        float time = 0;
        Vector3 startPosition = trans.position;

        while (time < duration)
        {
            float t = time / duration;
            t = t * t * (3f - 2f * t);
            trans.position = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        if (trans!= null)
            trans.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.N) && !floorsMoving)
            GenerateNextFloor();
    }


    GameObject InstantiateFloor(Vector3 location)
    {
        GameObject floor =
            Instantiate(FloorPrefab, position: location, Quaternion.identity);
        FloorMaster floorMaster = floor.GetComponentInChildren<FloorMaster>();
        if (floorCount > 0)
        {
            floorMaster.SpawnFloorContent(firstFloor: false);
        }
        else
            floorMaster.SpawnFloorContent(firstFloor: true);

        return floor;
    }


    private IEnumerator NewFloorSequence()
    {
        floorCount += 1;

        //Pause gameplay
        Player.GetComponent<PauseHandler>().isPaused = true;

        // Generate next floor
        nextFloor = InstantiateFloor(lowerFlr);
        FloorMaster newFloorMaster = nextFloor.GetComponentInChildren<FloorMaster>();
        oldFloor = currentFloor;

        // Move Floors up
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var proj in projectiles)
            Destroy(proj);
        floorsMoving = true;
        Vector3 plyrSpnPt = newFloorMaster.playerSpawnPoint.transform.position;
        StartCoroutine(LerpPosition(nextFloor.transform, mainFlr, floorChangeDuration));
        StartCoroutine(LerpPosition(oldFloor.transform, upperFlr, floorChangeDuration));
        StartCoroutine(LerpPosition(
            Player.transform,
            new Vector3(x: plyrSpnPt.x, y: Player.transform.position.y, plyrSpnPt.z),
            floorChangeDuration)
        );
        yield return new WaitForSeconds(floorChangeDuration);


        // Unpase gameplay
        newFloorMaster.PauseEnemies(false);
        // TODO: Get rid of third person reference
        Player.GetComponent<PauseHandler>().isPaused = false;
        Player.GetComponent<ThirdPersonHealth>().ResetHealth();
        Player.GetComponent<ThirdPersonDash>().ResetDash();

        // change references and cleanup
        currentFloor = nextFloor;
        nextFloor = null;
        floorsMoving = false;
        Destroy(oldFloor);
        Debug.Log("Floor number "+floorCount);
    }
}
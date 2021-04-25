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


    private GameObject currentFloor;
    private GameObject nextFloor;
    private GameObject oldFloor;

    public bool testing;

    public bool floorsMoving = false;

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
        currentFloor = Instantiate(
            FloorPrefab, position: mainFlr, Quaternion.identity
        );
    }

    void GenerateNextFloor()
    {
        // Generate next floor
        nextFloor = Instantiate(FloorPrefab, position: nextFloorT.position,
            Quaternion.identity);
        oldFloor = currentFloor;

        // Move Floors up
        floorsMoving = true;
        StartCoroutine(LerpPosition(nextFloor.transform, mainFlr, floorChangeDuration, false));
        StartCoroutine(LerpPosition(oldFloor.transform, upperFlr, floorChangeDuration, true));

        // change references
        currentFloor = nextFloor;
        nextFloor = null;
    }


    IEnumerator LerpPosition(Transform trans, Vector3 targetPosition, float duration,
        bool killAfterCompletion)
    {
        Debug.Log(trans.name + " is moving up");
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

        trans.position = targetPosition;

        if (killAfterCompletion)
            Destroy(trans.gameObject);

        floorsMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.N) && !floorsMoving)
            GenerateNextFloor();
    }
}
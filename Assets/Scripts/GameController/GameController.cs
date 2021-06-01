using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private FloorSpawner FloorSpawner;
    private FloorMaster CurrentFloorMaster;

    public Animator FloorTextAnimator;
    public TMP_Text FloorText;
    public TMP_Text ScoreText;
    
    public Animator GameOverAnimator;

    private bool haveNotGoneToNextFloor = true;

    public bool readyToStart = false;

    public float timeToStart = 2.8f; 
    

    // Start is called before the first frame update
    void Start()
    {
        FloorSpawner = FindObjectOfType<FloorSpawner>();
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(timeToStart);
        readyToStart = true;
    }


    void UpdateFloors()
    {
        CurrentFloorMaster =
            FloorSpawner.currentFloor.GetComponentInChildren<FloorMaster>();
        if (CurrentFloorMaster != null)
        {
            if (CurrentFloorMaster.allEnemiesDead)
            {
                // generate new floor
                FloorSpawner.GenerateNextFloor();
                FloorText.text = "Floor " + FloorSpawner.floorCount.ToString();
                FloorTextAnimator.SetTrigger("ZoomText");
//                Debug.Log("GO TO NEXT FLOOR");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToStart)
        {
            UpdateFloors();
        }

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    public void PlayerDied()
    {
        Debug.Log("PlayerDied called");
        GameOverAnimator.SetTrigger("GameOver");
        Cursor.lockState = CursorLockMode.None;
        ScoreText.text = "Floors Descended: " + FloorSpawner.floorCount.ToString(); 
    }

    public void Reload()
    {
        Debug.Log("RELOAD");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        Debug.Log("GO HOME");
        SceneManager.LoadScene("StartScene");
    }
}
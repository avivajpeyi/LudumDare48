using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
 
    public GameObject OptionsMenu;
    public GameObject InstructionsMenu;




    private OptionsContainer optionsContainer;

    void Start()
    {
        optionsContainer = FindObjectOfType<OptionsContainer>();
        OptionsMenu.SetActive(false);
        InstructionsMenu.SetActive(false);
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowOptions()
    {
        OptionsMenu.SetActive(true);
        InstructionsMenu.SetActive(false);
    }
    

    public void ShowInstructions()
    {
        OptionsMenu.SetActive(false);
        InstructionsMenu.SetActive(true);
    }
}
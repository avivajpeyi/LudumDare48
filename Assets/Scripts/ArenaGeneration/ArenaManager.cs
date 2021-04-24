using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : MonoBehaviour
{
    public ArenaComponentsManager entityManager;


    // Start is called before the first frame update
    void Start()
    {
        AutomatedRestart();
    }


    void ResetArena()
    {
        Debug.Log("Arena Reset");
        entityManager.ResetArenaComponents();
    }


    void AutomatedRestart()
    {
        InvokeRepeating("ResetArena", time: 0.5f, repeatRate: 3);
    }
}
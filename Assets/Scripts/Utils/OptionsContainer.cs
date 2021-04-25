using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OptionsContainer : MonoBehaviour
{
    public int dashDuration = 1;
    public int dashCooldown = 2;
    public int playerHealth = 1;


    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("dont_destroy");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    
}
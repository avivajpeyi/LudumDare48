using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCam : MonoBehaviour
{
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = FindObjectOfType<Camera>();
        Canvas can = GetComponent<Canvas>();
        can.worldCamera = cam;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

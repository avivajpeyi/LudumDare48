using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ThirdPersonMovement))]
public class ThirdPersonDash : MonoBehaviour
{
    public float dashSpeed = 20.0f;
    public float dashTime = 0.25f;
    public bool isDashing;
    
    private ThirdPersonMovement MovementController;

    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Dash Handler
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        Debug.Log("Start Dash @" + startTime );
        while (Time.time < startTime + dashTime)
        {
            // TODO: dash animation
            isDashing = true;
            MovementController.controller.Move(
                MovementController.moveDir * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
        Debug.Log("Complete Dash @" + Time.time );
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ThirdPersonMovement))]
public class ThirdPersonDash : MonoBehaviour
{
    public float dashSpeed = 20.0f;
    public float dashTime = 0.25f;

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
        Debug.Log("DASH");
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            MovementController.controller.Move(
                MovementController.moveDir * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
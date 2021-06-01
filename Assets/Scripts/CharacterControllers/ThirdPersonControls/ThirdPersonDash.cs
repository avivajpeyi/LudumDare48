using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ThirdPersonMovement))]
public class ThirdPersonDash : MonoBehaviour
{
    public float dashSpeed = 20.0f;
    public float dashTime = 0.25f;
    public bool isDashing;

    public float EnergyLeft = 100;
    public float EnergyUsage = 30;
    public float EnergyRechargePerDeltaT = 5;


    public RectTransform DashBar;

    private ThirdPersonMovement MovementController;
    private PlayerController _controller;

    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Dash Handler
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) ||
            Input.GetKeyDown(KeyCode.RightShift))
        {
            if (EnergyLeft > EnergyUsage  && !isDashing)
            {
                EnergyLeft -= EnergyUsage;
                StartCoroutine(Dash());
            }
        }

        RechargeEnergyUsage();
        AnimateDashBar();
        _controller.isDashing = isDashing;
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        Debug.Log("Start Dash @" + startTime);
        while (Time.time < startTime + dashTime)
        {
            // TODO: dash animation
            isDashing = true;
            MovementController.controller.Move(
                MovementController.moveDir * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        Debug.Log("Complete Dash @" + Time.time);
    }


    void AnimateDashBar()
    {
        if (DashBar != null)
        {
            DashBar.sizeDelta = Lerp(
                DashBar.sizeDelta,
                new Vector2(EnergyLeft - .5f, DashBar.sizeDelta.y), 
                Time.deltaTime * 6f
                );
        }
    }

    Vector2 Lerp(Vector2 a, Vector2 b, float d)
    {
        return new Vector2(Mathf.Lerp(a.x, b.x, d), Mathf.Lerp(b.x, b.y, d));
    }

    void RechargeEnergyUsage()
    {
        EnergyLeft += Time.deltaTime * EnergyRechargePerDeltaT;
        EnergyLeft = Mathf.Clamp(EnergyLeft, 0, 100);
    }

    public void ResetDash()
    {
        EnergyLeft = 100;
    }
}
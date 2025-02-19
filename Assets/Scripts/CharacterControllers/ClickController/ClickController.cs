﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Click3DCursor))]
public class ClickController : MonoBehaviour
{
    [Header("Movement Settings")] public float angularDrag = 0.05f;
    public float normalDrag = 4;
    public float moveSpeed = 10;

    [Tooltip("Distance to stop from Target")]
    public float stoppingDistance = 0.4f;


    [Header("Dash Settings")] public float dashSpeed = 30f;
    public float dashDuration = 1.5f;
    public float dashChargeDuration = 2.0f;


    private ClickArrowHandler _arrowHandler;
    private Click3DCursor _cursor3d;
    private GroundChecker _groundChecker;

    private TrailRenderer _trail;
    private Rigidbody _body;

    private Vector3 targetLocation = Vector3.zero;
    private Vector3 rollDir = Vector3.zero;

    public float cameraShakeAmount = 0.75f;
    public bool targetSet = false;

    [Header("Checks")] [SerializeField] public bool _canDash;
    [SerializeField] private bool _canMove;
    [SerializeField] public bool isDashing = false;
    [SerializeField] public bool isCharging = false;

    private PlayerController _controller;
    
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        ResetRigidBody();
        _groundChecker = FindObjectOfType<GroundChecker>();
        _arrowHandler = GetComponentInChildren<ClickArrowHandler>();
        _cursor3d = GetComponent<Click3DCursor>();
        _trail = GetComponent<TrailRenderer>();
        _arrowHandler.chargeDuration = dashChargeDuration;
        _cursor3d.InitCursor(_groundChecker.transform.position);
        _controller = GetComponent<PlayerController>();
    }


    void ResetRigidBody()
    {
        _body.useGravity = true;
        _body.drag = normalDrag;
        _body.angularDrag = angularDrag;
        _body.constraints = RigidbodyConstraints.None;
    }


    /// <summary>
    /// If cursor clicks ground, update target to location of click 
    /// </summary>
    void UpdateTargetRollDir()
    { 
        targetSet = _cursor3d.TargetSet();
        if (targetSet)
        {
            targetLocation = _cursor3d.GetTargetPosition();
            rollDir = targetLocation - transform.position;
            rollDir = Vector3.ProjectOnPlane(rollDir, Vector3.up);
            rollDir = rollDir.normalized;
        }
    }

    void UpdateArrowDirection()
    {
        _arrowHandler.target = targetLocation;
    }


    void Move()
    {
        _body.AddForce(rollDir * moveSpeed);
    }


    IEnumerator Dash()
    {
        // Dash initialisation
        isDashing = true;
        _trail.time = 1;
        CameraShake.Shake(0.1f, cameraShakeAmount);
        _body.drag = 0;
        _body.useGravity = false;
        _body.constraints = RigidbodyConstraints.FreezePositionY;
        _body.AddForce(rollDir * dashSpeed, ForceMode.Impulse);
        
        // Dash Cleanup
        rollDir = Vector3.zero;
        targetLocation = Vector3.zero;
        yield return new WaitForSeconds(dashDuration);
        ResetRigidBody();
        isDashing = false;
        _trail.DOTime(endValue: 0.0f, duration: 1f);
    }


    bool CanDashCheck()
    {
        return (_arrowHandler.reachedMaxScale && !isDashing && isCharging);
    }


    bool CanMoveCheck()
    {
        float distanceToTarget = Vector3.Distance(targetLocation, transform.position);
        return (
            !isDashing &&
            targetSet &&
            distanceToTarget > stoppingDistance
        );
    }

    void Update()
    {
        if (isDashing)
        {
            _arrowHandler.enabled = false;
        }
        else
        {
            _arrowHandler.enabled = true;
        }

        UpdateTargetRollDir();
        _canDash = CanDashCheck();
        _canMove = CanMoveCheck();

        if (Input.GetMouseButton(0))
        {
            if (_canMove)
            {
                isCharging = true;
                UpdateArrowDirection();
                Move();
            }


            targetSet = false;
        }


        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            if (_canDash)
            {
                StartCoroutine(Dash());
            }
        }

        _controller.isDashing = isDashing;
    }
    
    private void OnDestroy ()
    {
        Cursor.visible = true;
        if (_groundChecker != null)
        {
            Destroy(_groundChecker.gameObject);
            Destroy(_cursor3d.placedTarget);
            Destroy(_cursor3d.hoverTarget);
        }
        
    }


}
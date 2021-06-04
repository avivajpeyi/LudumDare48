using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayerAnimator : MonoBehaviour
{
    Vector3 rot = Vector3.zero;
    float rotSpeed = 40f;
    Animator anim;

    public float minSpeedToWalk = 2.5f;

    public float mag;
    
    private bool alreadyOpen = true;
    private ClickController _controller;
    private Rigidbody _body;
    
    // Use this for initialization
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameObject.transform.eulerAngles = rot;
        _controller = transform.root.GetComponent<ClickController>();
        _body = transform.root.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
        RotateFaceToTarget();
        gameObject.transform.eulerAngles = rot;
    }

    void RotateFaceToTarget()
    {
        
        

        
//        // The step size is equal to speed times frame time.
//        float singleStep = rotSpeed * Time.deltaTime;
//
//        // Rotate the forward vector towards the target direction by one step
//        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _controller.targetLocation, singleStep, 0.0f);
//
//        // Draw a ray pointing at our target in
//        Debug.DrawRay(transform.position, newDirection, Color.red);
//
//        // Calculate a rotation a step closer to the target and applies rotation to this object
//        transform.rotation = Quaternion.LookRotation(newDirection);
        if (_body.velocity != Vector3.zero)
        {
            Vector3 DesiredRotation = Quaternion.LookRotation(_body.velocity).eulerAngles;
//            Vector3 RotationSteering = DesiredRotation - transform.rotation.eulerAngles;
            rot[1] = DesiredRotation.y;
        }
    }
    

    void CheckKey()
    {
        mag = _body.velocity.magnitude;
        // walk anim
        if(_body.velocity.magnitude > minSpeedToWalk && !_controller.isDashing) 
        {
            anim.SetBool("Walk_Anim", true);
            anim.SetBool("Roll_Anim", false);
        }
        else if (_controller.isDashing)
        {
            anim.SetBool("Walk_Anim", false);
            anim.SetBool("Roll_Anim", true);
        }
        else if (_body.velocity.magnitude<= minSpeedToWalk)
        {
            anim.SetBool("Walk_Anim", false);
            anim.SetBool("Roll_Anim", false);
        }
        
        
    }
}

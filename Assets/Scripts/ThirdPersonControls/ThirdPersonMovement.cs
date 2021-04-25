using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Transform cam;


    //Dash & Movement
    public Vector3 moveDir;

    private PauseHandler myPause;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        cam = FindObjectOfType<Camera>().transform;
        myPause = GetComponent<PauseHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPause.isPaused)
        {
            Vector3 dir = GetUserDirections();

            if (dir.magnitude >= 0.1f)
            {
                MoveCharacter(dir);
            }
        }
    }


    Vector3 GetUserDirections()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0, v).normalized;
    }

    void MoveCharacter(Vector3 dir)
    {
        float targetAngle =
            Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
            ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }
}
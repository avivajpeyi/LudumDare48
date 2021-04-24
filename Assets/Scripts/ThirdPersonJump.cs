using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonJump : MonoBehaviour
{
    Vector3 velocity;
    public float gravity = -9.8f;
    public Transform groundCheck;
    public float groundDist;
    public LayerMask groundMask;
    [SerializeField] bool isGrounded;
    public float jumpHeight = 3;

    private ThirdPersonMovement MovementController;

    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpHandler();
    }

    void JumpHandler()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("JUMP!");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        MovementController.controller.Move(velocity * Time.deltaTime);
    }
}

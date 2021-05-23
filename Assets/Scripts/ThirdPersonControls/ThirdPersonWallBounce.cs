using UnityEngine;


// TODO: this should be done in the dash 

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonWallBounce : MonoBehaviour
{
    [SerializeField] private float minVelocity = 10f;


    [SerializeField] private Vector3 lastFrameVelocity;
    private Rigidbody rb;
    Vector3 velocity;

    private ThirdPersonMovement MovementController;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        MovementController = GetComponent<ThirdPersonMovement>();
    }

    private void Update()
    {
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision occured");
        if (collision.gameObject.CompareTag("Obstacle"))
            WallBounce(collision.contacts[0].normal);
    }


    private void WallBounce(Vector3 collisionNormal)
    {
        Debug.Log("Start Wall bounce");
        

        if (MovementController != null)
        {
            var direction = Vector3.Reflect(MovementController.moveDir, collisionNormal);
            velocity = direction * Mathf.Max(MovementController.speed, minVelocity);
            MovementController.controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            rb.AddForce(Vector3.Reflect(transform.forward.normalized, collisionNormal) 
            * lastFrameVelocity.magnitude, 
            ForceMode.Impulse);
    
        }
        
    }
}
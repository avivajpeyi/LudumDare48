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
        WallBounce(collision.contacts[0].normal);
    }
    

    private void WallBounce(Vector3 collisionNormal)
    {
        Debug.Log("Start Wall bounce");
        var direction = Vector3.Reflect(MovementController.moveDir, collisionNormal);

        Debug.Log("Bounce Out Direction: " + direction);
        velocity = direction * Mathf.Max(MovementController.speed, minVelocity);
        MovementController.controller.Move(velocity * Time.deltaTime);
        
    }
}
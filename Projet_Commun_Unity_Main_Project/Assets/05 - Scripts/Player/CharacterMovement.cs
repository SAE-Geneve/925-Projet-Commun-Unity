using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Physic")]
    [SerializeField] private float speed = 5f;
    
    protected Rigidbody Rb;

    protected Vector2 Movement;
   
    // Camera Directions
    protected Vector3 CamForward;
    protected Vector3 CamRight;
    
    
    protected void Start()
    {
        Rb = GetComponent<Rigidbody>();
        GetCameraDirections();
    }
    
    void Update()
    {
        RotateCharacter();
    }
    
    void FixedUpdate()
    {
        HorizontalMovement();
    }

    protected virtual void HorizontalMovement()
    {
        // Calculate the change in velocity needed and apply it (no acceleration or deceleration)
        Vector3 moveDir = CamRight * Movement.x + CamForward * Movement.y;
        
        Vector3 targetVelocity = moveDir * speed;
        Vector3 velocity = Rb.linearVelocity;
        
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);
        
        Rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    
    private void RotateCharacter()
    {
        // Rotate character to face move direction
        Vector3 moveDir = CamRight * Movement.x + CamForward * Movement.y;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
    }
    
    protected void GetCameraDirections()
    {
        // Calculate directions relative to the camera (static camera)
        CamForward = Camera.main.transform.forward;
        CamForward.y = 0;
        CamForward.Normalize();

        CamRight = Camera.main.transform.right;
        CamRight.y = 0;
        CamRight.Normalize();
    }


    public void SetMovement(Vector2 move)
    {
        Movement = move;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}

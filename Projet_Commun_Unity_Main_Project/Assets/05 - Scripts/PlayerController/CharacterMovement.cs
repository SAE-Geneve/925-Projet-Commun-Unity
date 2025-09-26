using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.4f;

    private Rigidbody _rb;
    private InputManager _inputs;

    private bool _isGrounded;
    private bool _wasGrounded;
    private readonly float _gravity = -20f;

    private Vector3 _camForward;
    private Vector3 _camRight;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputs = GetComponent<InputManager>();
        
        // Calculate directions relative to the camera (static camera)
        _camForward = Camera.main.transform.forward;
        _camForward.y = 0;
        _camForward.Normalize();

        _camRight = Camera.main.transform.right;
        _camRight.y = 0;
        _camRight.Normalize();
    }

    void Update()
    {
        // Ground check using Raycast
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        // Jump logic (trigger once when landing if button held)
        if (_inputs.JumpInput && _isGrounded && !_wasGrounded)
        {
            Jump();
        }

        _wasGrounded = _isGrounded;

        // Rotate character to face move direction
        Vector3 moveDir = _camRight * _inputs.MoveInput.x + _camForward * _inputs.MoveInput.y;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
    }

    void FixedUpdate()
    {
        // Calculate the change in velocity needed and apply it (no acceleration or deceleration)
        Vector3 moveDir = _camRight * _inputs.MoveInput.x + _camForward * _inputs.MoveInput.y;
        
        Vector3 targetVelocity = moveDir * speed;
        Vector3 velocity = _rb.linearVelocity;
        
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // Apply extra gravity manually for snappier jumps
        _rb.AddForce(Vector3.up * _gravity, ForceMode.Acceleration);
    }

    void Jump()
    {
        // Reset vertical velocity once before the jump
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
    }
}
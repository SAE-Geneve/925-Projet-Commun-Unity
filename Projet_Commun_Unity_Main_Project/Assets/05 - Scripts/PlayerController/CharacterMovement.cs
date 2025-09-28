using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Physic")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    
    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.4f;
    
    private Rigidbody _rb;
    
    private readonly float _gravity = -20f;
    
    private bool _isGrounded;
    private bool _jumpQueued;
    
    // Camera Directions
    private Vector3 _camForward;
    private Vector3 _camRight;
    
    // Inputs
    private bool _isJumping;
    private Vector2 _moveInput;
    
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public Vector2 MoveInput { get => _moveInput; set => _moveInput = value; }
    
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GetCameraDirections();
    }
    
    void Update()
    {
        RotateCharacter();

        // Queue jump when input is pressed
        if (_isJumping)
        {
            _jumpQueued = true;
        }
        else
        {
            _jumpQueued = false;
        }
    }
    
    void FixedUpdate()
    {
        GroundCheck();
        HorizontalMovement();
        JumpLogic();

        // Apply extra gravity manually for snappier jumps
        _rb.AddForce(Vector3.up * _gravity, ForceMode.Acceleration);
    }
    
    private void GroundCheck()
    {
        // Ground check using Raycast
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        // Debug Ray
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, _isGrounded ? Color.green : Color.red);
    }
    
    private void HorizontalMovement()
    {
        // Calculate the change in velocity needed and apply it (no acceleration or deceleration)
        Vector3 moveDir = _camRight * _moveInput.x + _camForward * _moveInput.y;
        
        Vector3 targetVelocity = moveDir * speed;
        Vector3 velocity = _rb.linearVelocity;
        
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);
        
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    
    void Jump()
    {
        // Reset vertical velocity once before the jump
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
    }

    private void JumpLogic()
    {
        // If jump was queued and player is grounded, perform jump
        if (_jumpQueued && _isGrounded)
        {
            Jump();
            _jumpQueued = false; 
        }
    }

    private void RotateCharacter()
    {
        // Rotate character to face move direction
        Vector3 moveDir = _camRight * _moveInput.x + _camForward * _moveInput.y;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
    }
    
    private void GetCameraDirections()
    {
        // Calculate directions relative to the camera (static camera)
        _camForward = Camera.main.transform.forward;
        _camForward.y = 0;
        _camForward.Normalize();

        _camRight = Camera.main.transform.right;
        _camRight.y = 0;
        _camRight.Normalize();
    }
}

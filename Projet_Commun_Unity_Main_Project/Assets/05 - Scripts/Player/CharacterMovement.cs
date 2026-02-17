using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Physic")]
    [SerializeField] protected float speed = 5f;
    public event Action<Vector3> OnMove;
    
    protected Rigidbody Rb;

    
    private Vector2 _movement;
   
    // Camera Directions
    private Vector3 _camForward;
    private Vector3 _camRight;
    
    public bool IsPushPull { get; set; }
    
    public bool FreeMovement { get; set; }
    
    public Vector2 Velocity => new (Rb.linearVelocity.x, Rb.linearVelocity.z);
    
    private Transform _mainCameraTransform;
    
    protected virtual void Start()
    {
        Rb = GetComponent<Rigidbody>();
        SetupCamera();
    }
    
    void Update()
    {
        UpdateCameraDirection();
        RotateCharacter();
    }
    
    void FixedUpdate()
    {
        HorizontalMovement();
    }

    protected virtual void HorizontalMovement()
    {
        if(FreeMovement) return;
        // Calculate the change in velocity needed and apply it (no acceleration or deceleration)
        Vector3 moveDir = _camRight * _movement.x + _camForward * _movement.y;
        
        Vector3 targetVelocity = moveDir * speed;
        Vector3 velocity = Rb.linearVelocity;
        
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);
        
        Rb.AddForce(velocityChange, ForceMode.VelocityChange);
        OnMove?.Invoke(moveDir);
    }
    
    private void RotateCharacter()
    {
        if(IsPushPull) return;
        
        // Rotate character to face move direction
        Vector3 moveDir = _camRight * _movement.x + _camForward * _movement.y;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
    }
    
    private void UpdateCameraDirection()
    {
        if(!_mainCameraTransform) return;
        // Calculate directions relative to the camera (static camera)
        _camForward = _mainCameraTransform.forward;
        _camForward.y = 0;
        _camForward.Normalize();

        _camRight = _mainCameraTransform.right;
        _camRight.y = 0;
        _camRight.Normalize();
    }

    public void SetupCamera()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    public void SetMovement(Vector2 move)
    {
        _movement = move;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    protected void InvokeOnMove(Vector3 moveDir)
    {
        OnMove?.Invoke(moveDir);
    }

    public void DisableMovement()
    {
        _movement = Vector2.zero;
        Rb.linearVelocity = Vector2.zero;
    }
}

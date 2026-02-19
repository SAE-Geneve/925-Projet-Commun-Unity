using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMovement : MonoBehaviour,  IRespawnable
{
    [Header("Player Control")]
    [SerializeField] private float maxForwardSpeed = 30f;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float turnSpeed = 180f; // Vitesse de rotation en degrés par seconde
    
    [Header("Grip Settings")]
    [SerializeField] private float tireGripFactor = 5f; 
    
    private Rigidbody _rb;

    private float _verticalInput;
    private float _horizontalInput;
    
        
    private Vector3 _startPosition;
    private Vector3 _startRotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        
        _rb.centerOfMass = new Vector3(0, -0.5f, 0); 
        
                
        _startPosition = _rb.position;
        _startRotation = _rb.rotation.eulerAngles;
    }
    
    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
        ApplyTireGrip();
    }

    private void ApplyMovement()
    {
        _rb.AddForce(transform.forward * (_verticalInput * acceleration), ForceMode.Acceleration);
        
        if(_rb.linearVelocity.magnitude > maxForwardSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxForwardSpeed;
        }
    }

    private void ApplyRotation()
    {
        float turnAmount = _horizontalInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnOffset = Quaternion.Euler(0f, turnAmount, 0f);
        
        _rb.MoveRotation(_rb.rotation * turnOffset);
        
        if (Mathf.Abs(_horizontalInput) < 0.1f)
        {
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void ApplyTireGrip()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_rb.linearVelocity);
        localVelocity.x = Mathf.Lerp(localVelocity.x, 0f, tireGripFactor * Time.fixedDeltaTime);
        _rb.linearVelocity = transform.TransformDirection(localVelocity);
    }
    
    public void Move(Vector2 input)
    {
        _horizontalInput = input.x;
        _verticalInput = input.y;
    }
    
    public void ResetInputs()
    {
        _verticalInput = 0f;
        _horizontalInput = 0f;
    }
    
    public void Respawn()
    {
        _rb.position = _startPosition;
        _rb.rotation = Quaternion.Euler(_startRotation);
    }
}
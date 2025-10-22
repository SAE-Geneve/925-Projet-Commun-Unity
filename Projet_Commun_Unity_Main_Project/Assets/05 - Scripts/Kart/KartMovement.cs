using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMovement : MonoBehaviour
{
    [Header("Player Control")]
    [SerializeField] private float maxForwardSpeed = 100f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float turnSpeed = 10f;
    
    private Rigidbody _rb;

    private float _verticalInput;
    private float _horizontalInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    
    void FixedUpdate()
    {
        Vector3 forwardVelocity = transform.forward * (_verticalInput * maxForwardSpeed);
        Vector3 velocityChange = forwardVelocity - _rb.linearVelocity;

        Vector3 accelerationVector = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
        
        _rb.AddForce(accelerationVector, ForceMode.VelocityChange);
        _rb.AddTorque(Vector3.up * (_horizontalInput * (turnSpeed * Time.fixedDeltaTime)), ForceMode.VelocityChange);
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
}
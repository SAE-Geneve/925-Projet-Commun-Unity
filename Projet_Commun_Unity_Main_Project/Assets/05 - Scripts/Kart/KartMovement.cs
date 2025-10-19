using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMouvement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Player Control")]
    [SerializeField] private float maxForwardSpeed = 100f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float turnSpeed = 10f;

    private float _verticalInput;
    private float _horizontalInput;
    
    private KartController _kartController;

    void Start()
    {
        _kartController = GetComponent<KartController>();
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

      
    }
    void FixedUpdate()
    {
        
        if (!_kartController.IsControlled) return;

        Vector3 forwardVelocity = transform.forward * (_verticalInput * maxForwardSpeed);
        Vector3 velocityChange = forwardVelocity - rb.linearVelocity;

        Vector3 accelerationVector = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
        rb.AddForce(accelerationVector, ForceMode.VelocityChange);

        rb.AddTorque(Vector3.up * (_horizontalInput * (turnSpeed * Time.fixedDeltaTime)), ForceMode.VelocityChange);
    }

    void OnMove(InputValue value)
    {
        if (!_kartController.IsControlled) return;

        Vector2 input = value.Get<Vector2>();
        _horizontalInput = input.x;
        _verticalInput = input.y;
    }
    public void ResetInputs()
    {
        _verticalInput = 0f;
        _horizontalInput = 0f;
    }
    
}

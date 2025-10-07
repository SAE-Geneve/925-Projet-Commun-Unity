using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMouvement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Player Control")]
    [SerializeField]private float maxForwardSpeed = 100f;
    [SerializeField]private float acceleration = 25f;
    [SerializeField]private float turnSpeed = 10f;

    private float verticalInput;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        Vector3 forwardVelocity = transform.forward * (verticalInput * maxForwardSpeed);
        Vector3 velocityChange = forwardVelocity - rb.linearVelocity;

        Vector3 accelerationVector = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
        rb.AddForce(accelerationVector, ForceMode.VelocityChange);

        rb.AddTorque(Vector3.up * (horizontalInput * (turnSpeed * Time.fixedDeltaTime)), ForceMode.VelocityChange);
    }
    
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        horizontalInput = input.x;
        verticalInput = input.y;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMovement : MonoBehaviour, IRespawnable
{
    [Header("Player Control")] [SerializeField]
    private float maxForwardSpeed = 30f;

    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float rotationSmoothFactor = 0.2f;

    [Header("Grip Settings")] [SerializeField]
    private float tireGripFactor = 5f;

    private Rigidbody _rb;

    private float _verticalInput;
    private float _horizontalInput;

    // Computed each FixedUpdate from camera-relative inputs
    private Vector3 _moveDirection;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private float _tempMaxSpeed = 0;
    private float _tempRotationSpeed = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.centerOfMass = new Vector3(0, -0.5f, 0);

        _startPosition = _rb.position;
        _startRotation = _rb.rotation;
    }

    void FixedUpdate()
    {
        BuildMoveDirection();
        ApplyMovement();
        ApplyRotation();
        ApplyTireGrip();
    }

    // Project camera axes onto the XZ plane, combine with inputs
    private void BuildMoveDirection()
    {
        Transform cam = Camera.main.transform;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        // Flatten — kill vertical tilt so the kart never flies
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        _moveDirection = (camForward * _verticalInput + camRight * _horizontalInput);

        // Normalize only if magnitude > 1 (diagonal would be faster otherwise)
        if (_moveDirection.sqrMagnitude > 1f)
            _moveDirection.Normalize();
    }

    private void ApplyMovement()
    {
        _rb.AddForce(_moveDirection * acceleration, ForceMode.Acceleration);

        if (_rb.linearVelocity.magnitude > maxForwardSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxForwardSpeed;
        }
    }

    private void ApplyRotation()
    {
        if (_moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation,
            rotationSmoothFactor * Time.fixedDeltaTime * 60f));
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
        _rb.rotation = _startRotation;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void Breakdown()
    {
        _tempMaxSpeed = maxForwardSpeed;
        maxForwardSpeed = 0f;

        _tempRotationSpeed = rotationSmoothFactor;
        rotationSmoothFactor = 0f;
    }

    public void Restart()
    {
        maxForwardSpeed = _tempMaxSpeed;
        rotationSmoothFactor = _tempRotationSpeed;
    }
}
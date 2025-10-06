using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartPhysic : MonoBehaviour
{
    [Header("Flottaison")]
    public float floatHeight = 2f;
    public float floatForce = 50f;
    public LayerMask groundLayer;

    [Header("Roues / Suspension")]
    public Transform[] wheels;
    public float suspensionSpeed = 5f;
    public float wheelHeightOffset = 0.5f;

    [Header("Self-Righting")]
    public float rightingTorque = 10f;

    [Header("Tilt Physique")]
    public float maxTiltForce = 80f;
    public float tiltDuration = 0.3f;

    private Rigidbody rb;
    private Vector3[] localOffsets;
    private float tiltTimer = 0f;
    private float tiltDirection = 0f;
    private float moveInput = 0f;
    private float lastMoveInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        WheelOffset();
    }
    
    
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.y;
    }

    void FixedUpdate()
    {
        HoverForce();
        HandleTilt();
    }

    void HoverForce()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, groundLayer))
        {
            float distance = hit.distance;
            float forceFactor = floatHeight - distance;
            rb.AddForce(Vector3.up * forceFactor * floatForce, ForceMode.Acceleration);
            
            Vector3 torqueDir = Vector3.Cross(transform.up, hit.normal);
            rb.AddTorque(torqueDir * rightingTorque, ForceMode.Acceleration);
        }
    }

    void HandleTilt()
    {
        float deltaInput = moveInput - lastMoveInput;
        if (Mathf.Abs(moveInput) > 0.1f && Mathf.Abs(deltaInput) > 0.01f)
        {
            tiltDirection = -Mathf.Sign(moveInput);
            tiltTimer = tiltDuration;

            float tiltForce = Mathf.Clamp(Mathf.Abs(deltaInput) * maxTiltForce, 0f, maxTiltForce);
            rb.AddTorque(transform.right * tiltDirection * tiltForce, ForceMode.Acceleration);
        }

        if (tiltTimer > 0f)
            tiltTimer -= Time.fixedDeltaTime;

        lastMoveInput = moveInput;
    }

    void Update()
    {
        UpdateSuspension();
    }

    void UpdateSuspension()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            Vector3 basePos = transform.TransformPoint(localOffsets[i]);
            Vector3 targetPos = WheelTargetPos(basePos);
            wheels[i].position = Vector3.Lerp(wheels[i].position, targetPos, Time.deltaTime * suspensionSpeed);
        }
    }
    
    Vector3 WheelTargetPos(Vector3 basePos)
    {
        if (Physics.Raycast(basePos + Vector3.up, Vector3.down, out RaycastHit hit, 10f, groundLayer))
        {
            float groundY = hit.point.y + wheelHeightOffset;
            float targetY = Mathf.Max(groundY, basePos.y);
            Vector3 targetPos = basePos;
            targetPos.y = targetY;
            return targetPos;
        }
        return basePos;
    }
    
    void WheelOffset()
    {
        localOffsets = new Vector3[wheels.Length];
        for (int i = 0; i < wheels.Length; i++)
            localOffsets[i] = transform.InverseTransformPoint(wheels[i].position);
    }
}

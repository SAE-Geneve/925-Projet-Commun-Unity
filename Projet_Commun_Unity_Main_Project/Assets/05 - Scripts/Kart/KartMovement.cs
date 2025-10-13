using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMouvement : MonoBehaviour, IInteractable
{
    private Rigidbody rb;

    [Header("Player Control")]
    [SerializeField] private float maxForwardSpeed = 100f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float turnSpeed = 10f;

    private float verticalInput;
    private float horizontalInput;

    [Header("Input")]
    public PlayerInput vehicleInput;
    public Transform seatPosition;

    private bool isControlled = false;
    private GameObject currentDriver;
    private MonoBehaviour driverInputHandler;
    
    
        

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (vehicleInput != null)
            vehicleInput.enabled = false;
    }

    void FixedUpdate()
    {
        if (!isControlled) return;

        Vector3 forwardVelocity = transform.forward * (verticalInput * maxForwardSpeed);
        Vector3 velocityChange = forwardVelocity - rb.linearVelocity;

        Vector3 accelerationVector = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
        rb.AddForce(accelerationVector, ForceMode.VelocityChange);

        rb.AddTorque(Vector3.up * (horizontalInput * (turnSpeed * Time.fixedDeltaTime)), ForceMode.VelocityChange);
    }

    void OnMove(InputValue value)
    {
        if (!isControlled) return;

        Vector2 input = value.Get<Vector2>();
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    // ===============================
    //     IInteractable interface
    // ===============================
    public void Interact(GameObject interactor)
    {   
        Debug.Log("INTERAGIT");
        if (!isControlled)
            EnterVehicle(interactor);
        else if (interactor == currentDriver)
            ExitVehicle();
    }

    private void EnterVehicle(GameObject interactor)
    {
        currentDriver = interactor;
        isControlled = true;
        
        driverInputHandler = interactor.GetComponentInChildren<MonoBehaviour>();
        if (driverInputHandler != null)
            driverInputHandler.enabled = false;

        interactor.SetActive(false);

        if (vehicleInput != null)
            vehicleInput.enabled = true;

        interactor.transform.position = seatPosition.position;
    }

    private void ExitVehicle()
    {
        if (currentDriver == null) return;

        isControlled = false;

        currentDriver.SetActive(true);
        currentDriver.transform.position = transform.position + transform.right * 2f;

        if (driverInputHandler != null)
            driverInputHandler.enabled = true;

        if (vehicleInput != null)
            vehicleInput.enabled = false;

        verticalInput = 0f;
        horizontalInput = 0f;
        currentDriver = null;
        driverInputHandler = null;
    }
}

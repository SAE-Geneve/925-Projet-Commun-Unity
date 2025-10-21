using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartMovement : MonoBehaviour
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
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    
    void FixedUpdate()
    {
        // Si le kart n'est pas contrôlé, on ne fait rien
        if (!_kartController.IsControlled) return;

        Vector3 forwardVelocity = transform.forward * (_verticalInput * maxForwardSpeed);
        Vector3 velocityChange = forwardVelocity - rb.linearVelocity;

        Vector3 accelerationVector = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
        rb.AddForce(accelerationVector, ForceMode.VelocityChange);

        rb.AddTorque(Vector3.up * (_horizontalInput * (turnSpeed * Time.fixedDeltaTime)), ForceMode.VelocityChange);
    }
    
    // Cette méthode est maintenant appelée par le PlayerInput du joueur
    public void OnMove(InputAction.CallbackContext context)
    {
        // Si l'Action est activée (bouton pressé ou joystick déplacé)
        if (context.performed)
        {
            Vector2 input  = context.ReadValue<Vector2>();
            _horizontalInput = input.x;
            _verticalInput = input.y;
        }
        // Si l'Action est annulée (bouton relâché ou joystick centré)
        else if (context.canceled)
        {
            // Remet les inputs à zéro pour arrêter le mouvement et la rotation
            _verticalInput = 0f;
            _horizontalInput = 0f;
        }
    }
    
    public void ResetInputs()
    {
        _verticalInput = 0f;
        _horizontalInput = 0f;
    }
}
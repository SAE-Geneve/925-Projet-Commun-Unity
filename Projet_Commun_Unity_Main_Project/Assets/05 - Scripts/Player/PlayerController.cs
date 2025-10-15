using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _catchPoint;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Slider _throwBar;
    [SerializeField] private Collider _collider;

    [Header("Throw bar")] 
    [SerializeField] private float _barSpeed = 2f;
    
    [Header("Throw")]
    [SerializeField] private float _throwThreshold = 1f;
    [SerializeField] private float _maxThrowForce = 10f;
    [SerializeField] private float _upForceMultiplier = 1f;
    
    [Header("Catch/Interact")]
    [SerializeField] private float _sphereRadius = 0.2f;
    
    public Rigidbody Rb => _rb;
    public Collider Collider => _collider;
    public Transform CatchPoint => _catchPoint;
    public PlayerMovement PlayerMovement { get; private set; }
    
    private Ragdoll _ragdoll;
    
    Vector3 throwDirection;
    
    private IGrabbable _grabbed;
    
    private float _grabStartTime;
    private float _throwPower;
    
    private bool _isCharging;

    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        _ragdoll = GetComponent<Ragdoll>();

        //TODO: enlever quand on utilisera plus le capsule de la scene prop
        if(_ragdoll)_ragdoll.OnRagdoll += Drop;
        
        _throwBar.gameObject.SetActive(false);
    }

    public void Catch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(_grabbed == null) TryGrab();
            else
            {
                _isCharging = true;
                _grabStartTime = Time.time;
                _throwPower = 0f;
                _throwBar.gameObject.SetActive(true);
                _throwBar.value = 0f;
            }
        }
        else if (context.canceled)
        {
            if (_isCharging)
            {
                Drop();
            }
        }
    }

    private void Drop()
    {
        if (_grabbed != null)
        {
            _grabbed.Dropped(ThrowDirection(), this);
            _throwBar.gameObject.SetActive(false);
            _isCharging = false;
        }
    }

    private void Update()
    {
        if (_isCharging)
        {
            _throwPower = Mathf.PingPong((Time.time - _grabStartTime) * _barSpeed, _throwBar.maxValue);
            _throwBar.value = _throwPower;
        }
    }
    
    private Vector3 ThrowDirection()
    {
        Vector3 forward = transform.forward;
        Vector3 upward = transform.up * _upForceMultiplier;
        float force = _throwPower * _maxThrowForce;
        throwDirection = (forward + upward).normalized * force;
        
        return throwDirection + _rb.linearVelocity;
    }
   
    
    private void TryAction<T>(Action<T> onFound) where T : class
    {
        Collider[] hits = Physics.OverlapSphere(CatchPoint.position, _sphereRadius);
        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out T component)) continue;
            onFound(component);
            break;
            
        }
    }
    
    private void TryGrab()
    {
        TryAction<IGrabbable>(grabbable =>
        {
            grabbable.Grabbed(this);
            _grabbed = grabbable;
        });
    }

    public void TryInteract()
    {

        TryAction<IInteractable>(interactable =>
        {
            interactable.Interact(gameObject);
        });
    }
    
    public void Reset() => _grabbed = null;
    
    private void OnDrawGizmos()
    {
        if (!CatchPoint) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CatchPoint.position, _sphereRadius);
    }
}
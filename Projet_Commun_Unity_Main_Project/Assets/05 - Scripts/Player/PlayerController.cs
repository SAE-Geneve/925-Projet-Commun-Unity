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

    [Header("Throw bar")] 
    [SerializeField] private float _barSpeed = 2f;
    
    [Header("Throw")]
    [SerializeField] private float _throwThreshold = 1f;
    [SerializeField] private float _maxThrowForce = 10f;
    [SerializeField] private float _upForceMultiplier = 1f;
    
    [Header("Catch/Interact")]
    [SerializeField] private float _sphereRadius = 0.2f;
    
    public Rigidbody Rb => _rb;
    public Transform CatchPoint => _catchPoint;
    
    Vector3 throwDirection;
    
    private IGrabbable _grabbed;
    
    private float _grabStartTime;
    private float _throwPower;
    
    private bool _isCharging;

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
                if(_throwBar != null) _throwBar.value = 0f;
            }
        }
        else if (context.canceled && _grabbed != null && _isCharging)
        {
            _grabbed.Dropped(ThrowDirection());
            _isCharging = false;
        }
    }

    private void Update()
    {
        if (_isCharging)
        {
            _throwPower = Mathf.PingPong((Time.time - _grabStartTime) * _barSpeed, _throwBar ? _throwBar.maxValue : 1f);
            if(_throwBar) _throwBar.value = _throwPower;
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
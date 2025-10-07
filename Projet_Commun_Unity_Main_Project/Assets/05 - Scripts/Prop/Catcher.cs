using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Catcher : MonoBehaviour
{
    public Transform CatchPoint;
    
    private IGrabbable _grabbed;
    private float grabStartTime;
    [SerializeField] private float throwThreshold = 1.0f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float maxThrowForce = 10f;
    [SerializeField] private Slider throwBar;
    Vector3 throwDirection;
    float heldTime;
    private float throwPower;
    private bool isCharging;

    public void Catch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grabStartTime = Time.time;
            isCharging = true;
        }
        if (context.canceled)
        {
            heldTime = Time.time - grabStartTime;
            isCharging = false;
        }
        if (_grabbed != null)
        {
            _grabbed.Dropped(ThrowDirection());
            throwPower = 0f;
        }
        else TryGrab();
    }

    private void Update()
    {
        if (isCharging)
        {
            throwPower = Mathf.PingPong((Time.time - grabStartTime) * 2, 1f);
            throwBar.value = throwPower;
        }
        else
        {
            throwBar.value = 0f;
        }
    }
    
    private Vector3 ThrowDirection()
    {
        Vector3 forward = transform.forward;
        Vector3 upward = transform.up * 0.5f;
        float force = throwPower * maxThrowForce;
        throwDirection = (forward + upward).normalized * force;
        
        return throwDirection + _rb.linearVelocity;
    }
   
    
    private void TryAction<T>(Action<T> onFound) where T : class
    {
        Collider[] hits = Physics.OverlapSphere(CatchPoint.position, .2f);
    
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
            interactable.Interact();
        });
    }
    
    public void Reset() => _grabbed = null;
    
    private void OnDrawGizmos()
    {
        if (!CatchPoint) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CatchPoint.position, 0.2f);
    }
}
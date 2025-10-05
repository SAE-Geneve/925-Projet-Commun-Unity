using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    public Transform CatchPoint;
    
    private IGrabbable _grabbed;
    private float grabStartTime;
    [SerializeField] private float throwThreshold = 1.0f;
    Vector3 throwDirection;

    public void CatchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grabStartTime = Time.time;
        }
        if (context.canceled)
        {
            float heldTime = Time.time - grabStartTime;
            Vector3 forward = transform.forward;
            Vector3 upward = transform.up * 0.5f;
            throwDirection = (forward + upward).normalized * heldTime * 6;
        }
        if (_grabbed != null)
        {
            _grabbed.Dropped(throwDirection);
        }
        else TryGrab();
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
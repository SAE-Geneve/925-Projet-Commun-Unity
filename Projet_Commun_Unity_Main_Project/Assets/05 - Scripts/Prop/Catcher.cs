using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    public Transform CatchPoint;
    
    private IGrabbable _grabbed;

    public void CatchInput(InputAction.CallbackContext context)
    {
        if (_grabbed != null) _grabbed.Dropped();
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
}
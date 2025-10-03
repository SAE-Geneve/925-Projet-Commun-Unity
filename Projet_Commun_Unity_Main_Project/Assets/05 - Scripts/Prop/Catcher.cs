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

    private void TryGrab()
    {
        Collider[] hits = Physics.OverlapSphere(CatchPoint.position, .1f);
        
        foreach (var hit in hits)
        {
            IGrabbable grabbable = hit.GetComponent<IGrabbable>();
            grabbable.Grabbed(this);
            _grabbed = grabbable;
            break;
        }
    }

    public void TryInteract()
    {
        Collider[] hits = Physics.OverlapSphere(CatchPoint.position, .1f);
        
        foreach (var hit in hits)
        {
            if(!hit.TryGetComponent(out IInteractable interactable)) continue;
            
            interactable.Interact();
            return;
        }
    }
    
    public void Reset() => _grabbed = null;
}

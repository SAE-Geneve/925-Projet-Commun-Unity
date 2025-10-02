using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Catcher : MonoBehaviour
{
    
    [SerializeField] private Transform catchPoint;
    private IGrabbable _grabbed;

    public void CatchInput(InputAction.CallbackContext context)
    {
        if (_grabbed != null)
        {
            _grabbed.Dropped();
            _grabbed = null;
        }
        else
        {
            TryGrab();
        }
    }

    public void TryGrab()
    {
        Collider[] hits = Physics.OverlapSphere(catchPoint.position, .1f);
        foreach (var hit in hits)
        {
            IGrabbable grabbable = hit.GetComponent<IGrabbable>();
            grabbable.Grabbed(catchPoint);
            _grabbed = grabbable;
            break;
            
        }
    }
}

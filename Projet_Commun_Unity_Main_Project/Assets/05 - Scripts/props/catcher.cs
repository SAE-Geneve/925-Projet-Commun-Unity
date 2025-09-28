using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class catcher : MonoBehaviour
{
    
    [SerializeField] private Transform catchPoint;
    
    private Grabbable _catchedGrabbable;
    private Rigidbody _catchedGrabbableRb;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CatchInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            CatchBox();
        else if (context.canceled)
            ReleaseBox();
    }
    
    void CatchBox()
    {
        var hits = Physics.SphereCastAll(transform.position, 1, Vector3.forward);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Grabbable grabbable))
            {
                _catchedGrabbable = grabbable;
                ParentConstraint pc = _catchedGrabbable.AddComponent<ParentConstraint>();
                pc.constraintActive = true;
                ConstraintSource source = new ConstraintSource();
                source.sourceTransform = catchPoint;
                source.weight = 1;
                pc.AddSource(source);
                _catchedGrabbable.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }
        }
    }
    
    void ReleaseBox()
    {
        if (_catchedGrabbable != null)
        {
            ParentConstraint pc = _catchedGrabbable.GetComponent<ParentConstraint>();
            Destroy(pc);
            _catchedGrabbable.GetComponent<Rigidbody>().isKinematic = false;
            _catchedGrabbable = null;
        }
    }
}

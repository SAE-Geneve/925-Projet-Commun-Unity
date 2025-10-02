using UnityEngine;

public class ThrowableProp : Prop
{

    private Transform _originalParent;
    
    public override void Grabbed(Transform grabbedBy)
    {
        _originalParent = transform.parent;
        transform.SetParent(grabbedBy);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        if (_rb != null) _rb.isKinematic = true;
        Debug.Log("Grabbed movable object");
    }

    public override void Dropped()
    {
        transform.SetParent(_originalParent);
        if(_rb != null) _rb.isKinematic = false;
        Debug.Log("Dropped movable object");
    }
}

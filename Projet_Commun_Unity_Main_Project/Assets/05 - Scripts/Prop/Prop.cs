using UnityEngine;

public class Prop: MonoBehaviour, IGrabbable
{
    private Transform _originalParent;
    
    private Rigidbody _rb;
    private Catcher _catcher;
    
    public bool IsGrabbed { get; private set; }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public virtual void Grabbed(Catcher catcher)
    {
        IsGrabbed = true;
        _originalParent = transform.parent;
        transform.SetParent(catcher.CatchPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        _catcher = catcher;
        
        if (_rb != null) _rb.isKinematic = true;
        Debug.Log("Grabbed movable object");
    }

    public virtual void Dropped(Vector3 throwForce = default)
    {
        transform.SetParent(_originalParent);
        if(_rb != null) _rb.isKinematic = false;

        if (_catcher)
        {
            _catcher.Reset();
            _catcher = null;
        }

        if (throwForce != Vector3.zero)
        {
            _rb.AddForce(throwForce, ForceMode.Impulse);
        }
        IsGrabbed = false;
    }
}

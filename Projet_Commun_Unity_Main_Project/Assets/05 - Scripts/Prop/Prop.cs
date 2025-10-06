using UnityEngine;

public class Prop: MonoBehaviour, IGrabbable
{
    [Header("Parameters")]
    [Tooltip("Define the prop type of the game object")]
    [SerializeField] private PropType _type = PropType.None;
    
    public PropType Type => _type;
    public bool IsGrabbed { get; private set; }
    
    private Transform _originalParent;
    private Rigidbody _rb;
    private Catcher _catcher;
    
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
        Debug.Log("Grabbed object");
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

public enum PropType
{
    None,
    Luggage
}
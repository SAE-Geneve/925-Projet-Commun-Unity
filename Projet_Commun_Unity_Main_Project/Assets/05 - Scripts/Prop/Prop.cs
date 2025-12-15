using System;
using UnityEngine;

public class Prop: MonoBehaviour, IGrabbable, IRespawnable
{
    [Header("Parameters")]
    [Tooltip("Define the prop type of the game object")]
    [SerializeField] private PropType _type = PropType.None;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private float _minForceToThrow = 3.0f; 
    

    public event Action<Prop> OnDestroyed;
    
    public PropType Type => _type;
    public Rigidbody Rb => _rb;
    public bool IsGrabbed { get; protected set; }
    
    protected Rigidbody _rb;
    protected Collider _collider;
    
    protected Controller Controller;
    protected Transform _originalParent;
    
    // Respawn parameters
    Vector3 _respawnPosition;
    Quaternion _respawnRotation;
    Vector3 _respawnScale;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        
        _respawnPosition = transform.position;
        _respawnRotation = transform.rotation;
        _respawnScale = transform.localScale;
    }

    #region Grab

    public virtual void Grabbed(Controller controller)
    {
        if(IsGrabbed) Dropped();
        
        IsGrabbed = true;
        
        _originalParent = transform.parent;
        
        transform.SetParent(controller.CatchPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        Controller = controller;
        
        if (_rb) _rb.isKinematic = true;
    }

    public virtual void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        transform.SetParent(_originalParent);
        if(_rb) _rb.isKinematic = false;

        if (Controller)
        {
            Controller.Reset();
            Controller = null;
        }

        if (throwForce.magnitude < _minForceToThrow)
        {
            throwForce = Vector3.zero;
        }
        
        if (throwForce != Vector3.zero)
        {
            float curveValue = _speedCurve.Evaluate(throwForce.magnitude);
            Vector3 curvedForce = throwForce * curveValue;
            _rb.AddForce(curvedForce, ForceMode.Impulse);
        }
        IsGrabbed = false;
    }

    #endregion

    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
        if(IsGrabbed) Dropped();
        
        Destroy(gameObject);
    }

    private void OnDestroy() => OnDestroyed?.Invoke(this);
    
    public void SetType(PropType type) => _type = type;
    public void Respawn()
    {
        if(IsGrabbed) Dropped();
        transform.position = _respawnPosition;
        transform.rotation = _respawnRotation;
        transform.localScale = _respawnScale;
    }
}

public enum PropType
{
    None,
    RedLuggage,
    BlueLuggage,
    GreenLuggage,
    YellowLuggage,
    GoodProp,
    BadProp,
    StairKart,
    Trash
}
using System.Collections.Generic;
using UnityEngine;

public class Prop: MonoBehaviour, IGrabbable
{
    [Header("Parameters")]
    [Tooltip("Define the prop type of the game object")]
    [SerializeField] private PropType _type = PropType.None;


    public AnimationCurve speedCurve;
    public PropType Type => _type;
    public Rigidbody Rb => _rb;
    public bool IsGrabbed { get; protected set; }
    
    protected Rigidbody _rb;
    protected Controller Controller;
    
    protected Transform _originalParent;
    protected Collider _collider;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    #region Grab

    public virtual void Grabbed(Controller controller)
    {
        IsGrabbed = true;
        _originalParent = transform.parent;
        transform.SetParent(controller.CatchPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        Controller = controller;
        
        if (_rb != null) _rb.isKinematic = true;
        Debug.Log("Grabbed object");
    }

    public virtual void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        transform.SetParent(_originalParent);
        if(_rb != null) _rb.isKinematic = false;

        if (Controller)
        {
            Controller.Reset();
            Controller = null;
        }

        if (throwForce != Vector3.zero)
        {
            float curveValue = speedCurve.Evaluate(throwForce.magnitude);
            Vector3 curvedForce = throwForce * curveValue;
            _rb.AddForce(curvedForce, ForceMode.Impulse);
        }
        IsGrabbed = false;
    }

    #endregion
}

public enum PropType
{
    None,
    RedLuggage,
    BlueLuggage,
    GreenLuggage,
    YellowLuggage,
}
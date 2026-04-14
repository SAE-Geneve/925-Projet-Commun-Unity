using System;
using System.Collections;
using UnityEngine;

public class Prop: MonoBehaviour, IGrabbable, IRespawnable
{
    [Header("Parameters")]
    [Tooltip("Define the prop type of the game object")]
    [SerializeField] private PropType _type = PropType.None;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private float _minForceToThrow = 3.0f;

    [Header("Destroy Animation")]
    [SerializeField] private float _destroyAnimDuration = 0.5f;

    public event Action<Prop> OnDestroyed;
    public PropType Type => _type;
    public Rigidbody Rb => _rb;
    public bool IsGrabbed { get; protected set; }
    public bool IsStacked { get; set; }
    
    protected Rigidbody _rb;
    protected Collider _collider;

    [NonSerialized] public Controller Controller;
    protected Transform _originalParent;

    private ObjectOutline _outline;
    private PropFeedback _feedback;
    
    public int OwnerId { get; private set; }
    
    Vector3 _respawnPosition;
    Quaternion _respawnRotation;
    Vector3 _respawnScale;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _outline = GetComponent<ObjectOutline>();
        _feedback = GetComponent<PropFeedback>();
        
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
        
        if (controller is PlayerController pc)
            OwnerId = pc.Id;
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
            throwForce = Vector3.zero;
        
        if (throwForce != Vector3.zero)
        {
            float curveValue = _speedCurve.Evaluate(throwForce.magnitude);
            _rb.AddForce(throwForce * curveValue, ForceMode.Impulse);
            _feedback?.PlayThrowEffect();
        }

        IsGrabbed = false;
    }

    #endregion

    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
        if(IsGrabbed) Dropped();
        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        if (_collider) _collider.enabled = false;
        if (_rb) _rb.isKinematic = true;

        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < _destroyAnimDuration)
        {
            if (!this) yield break;
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / _destroyAnimDuration);
            yield return null;
        }

        Destroy(gameObject);
    }

    protected virtual void OnDestroy() => OnDestroyed?.Invoke(this);
    
    public void SetType(PropType type) => _type = type;

    public void Respawn()
    {
        if(IsGrabbed) Dropped();
        transform.position = _respawnPosition;
        transform.rotation = _respawnRotation;
        transform.localScale = _respawnScale;
    }

    public void AreaEnter()
    {
        if(_outline) _outline.EnableOutline();
    }

    public void AreaExit()
    {
        if(_outline) _outline.DisableOutline();
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
    Trash,
    MoneyBag
}
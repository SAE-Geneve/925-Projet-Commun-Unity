using UnityEngine;

public class Prop: MonoBehaviour, IGrabbable
{
    [Header("Parameters")]
    [Tooltip("Define the prop type of the game object")]
    [SerializeField] private PropType _type = PropType.None;
    
    public PropType Type => _type;
    public bool IsGrabbed { get; protected set; }
    
    protected Transform _originalParent;
    protected Rigidbody _rb;
    protected PlayerController PlayerController;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public virtual void Grabbed(PlayerController playerController)
    {
        IsGrabbed = true;
        _originalParent = transform.parent;
        transform.SetParent(playerController.CatchPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        PlayerController = playerController;
        
        if (_rb != null) _rb.isKinematic = true;
        Debug.Log("Grabbed object");
    }

    public virtual void Dropped(Vector3 throwForce = default)
    {
        transform.SetParent(_originalParent);
        if(_rb != null) _rb.isKinematic = false;

        if (PlayerController)
        {
            PlayerController.Reset();
            PlayerController = null;
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
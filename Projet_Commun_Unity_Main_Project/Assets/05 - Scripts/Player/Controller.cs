using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IGrabbable
{
    [Header("References")]
    [SerializeField] private Transform _catchPoint;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Slider _throwBar;
    [SerializeField] private Collider _collider;

    [Header("Throw bar")] 
    [SerializeField] private float _barSpeed = 2f;
    
    [Header("Throw")]
    [SerializeField] protected float _maxThrowForce = 10f;
    [SerializeField] private float _upForceMultiplier = 1f;
    
    [Header("Catch/Interact")]
    [SerializeField] private float _sphereRadius = 0.2f;
    
    public Rigidbody Rb => _rb;
    public Collider Collider => _collider;
    public Transform CatchPoint => _catchPoint;
    
    public CharacterMovement Movement { get; private set; }
    public CharacterDisplay Display { get; private set; }
    public IInteractable InteractableGrabbed { get; set; }
    
    public bool IsBeingHeld { get; private set; }
    
    private Ragdoll _ragdoll;
    private Vector3 throwDirection;
    
    private IGrabbable _grabbedObject;
    
    private float _grabStartTime;
    protected float _throwPower;
    
    private bool _isCharging;
    protected virtual void Start()
    {
        Movement = GetComponent<CharacterMovement>();
        Display = GetComponent<CharacterDisplay>();

        _ragdoll = GetComponent<Ragdoll>();

        _ragdoll.OnRagdoll += Drop;
        
        _throwBar.gameObject.SetActive(false);
    }
    
    public void CatchCanceled()
    {
        if (_isCharging) Drop();
    }

    public void CatchStart()
    {
        if(_grabbedObject as UnityEngine.Object == null) TryGrab();
        else
        {
            _isCharging = true;
            _grabStartTime = Time.time;
            _throwPower = 0f;
            _throwBar.gameObject.SetActive(true);
            _throwBar.value = 0f;
            Debug.Log(_grabbedObject);
        }
    }

    public void Drop()
    {
        if (_grabbedObject != null)
        {
            _grabbedObject.Dropped(ThrowDirection(), this);
            _grabbedObject = null;
        }
    }

    private void Update()
    {
        if (_isCharging)
        {
            _throwPower = Mathf.PingPong((Time.time - _grabStartTime) * _barSpeed, _throwBar.maxValue);
            _throwBar.value = _throwPower;
        }
    }
    
    private Vector3 ThrowDirection()
    {
        Vector3 forward = transform.forward;
        Vector3 upward = transform.up * _upForceMultiplier;
        throwDirection = (forward + upward).normalized * CalculateThrowForce();
        
        return throwDirection + _rb.linearVelocity;
    }

    protected virtual float CalculateThrowForce()
    {
        return _throwPower * _maxThrowForce;
    }


    protected void TryAction<T>(Action<T> onFound) where T : class
    {
        Collider[] hits = Physics.OverlapSphere(CatchPoint.position, _sphereRadius);
        
        float nearestDistance = Mathf.Infinity;
        T nearestComponent = null;
        
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject || !hit.TryGetComponent(out T component)) continue;
            
            float distance = Vector3.Distance(CatchPoint.position, hit.transform.position);
            
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestComponent = component;
            }
        }

        if (nearestComponent != null)
            onFound(nearestComponent);
    }
    
    
    private void TryGrab()
    {
        if (IsBeingHeld) return;
        
        TryAction<IGrabbable>(grabbable =>
        {
            grabbable.Grabbed(this);
            _grabbedObject = grabbable;
        });
    }

    public void Reset()
    {
        _grabbedObject = null;
        _throwBar.gameObject.SetActive(false);
        _isCharging = false;
    }

    private void OnDrawGizmos()
    {
        if (!CatchPoint) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CatchPoint.position, _sphereRadius);
    }

    public void Grabbed(Controller controller)
    {
        Debug.Log("Grabbed");
        
        if (_grabbedObject != null)
        {
            Drop(); 
        }
        IsBeingHeld = true; 
        
        transform.SetParent(controller.CatchPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Rb.isKinematic = true;
        if(Movement) Movement.enabled = false;
    }

    public void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        IsBeingHeld = false;

        ResetParent();

        Rb.isKinematic = false;
        
        if(Movement) Movement.enabled = true;
        
        if (throwForce != Vector3.zero)
        {
            Movement.FreeMovement = true;
            _rb.AddForce(throwForce, ForceMode.Impulse);
        }

        StartCoroutine(DropRoutine());
        
        if (controller) controller.Reset();
        
        Debug.Log("Dropped controller");
    }

    public void ResetParent() => transform.SetParent(PlayerManager.Instance ? PlayerManager.Instance.transform : null);
    public IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _ragdoll.RagdollOn();
        Movement.FreeMovement = false;
    }

    private void OnDestroy()
    {
        Drop();
    }

    public void SetGrabbedProp(Prop prop) => _grabbedObject = prop;
}
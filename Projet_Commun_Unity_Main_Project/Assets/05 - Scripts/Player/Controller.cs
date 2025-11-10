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
    [SerializeField] private float _maxThrowForce = 10f;
    [SerializeField] private float _upForceMultiplier = 1f;
    
    [Header("Catch/Interact")]
    [SerializeField] private float _sphereRadius = 0.2f;
    
    public Rigidbody Rb => _rb;
    public Collider Collider => _collider;
    public Transform CatchPoint => _catchPoint;
    public CharacterMovement Movement { get; private set; }
    
    public IInteractable InteractableGrabbed { get; set; }
    
    private Ragdoll _ragdoll;
    
    private Vector3 throwDirection;
    
    private IGrabbable _grabbed;
    
    private float _grabStartTime;
    private float _throwPower;
    
    private bool _isCharging;
    
    private Transform _originalParent;

    protected virtual void Start()
    {
        Movement = GetComponent<CharacterMovement>();
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
        if(_grabbed == null) TryGrab();
        else
        {
            _isCharging = true;
            _grabStartTime = Time.time;
            _throwPower = 0f;
            _throwBar.gameObject.SetActive(true);
            _throwBar.value = 0f;
        }
    }

    public void Drop()
    {
        if (_grabbed != null) _grabbed.Dropped(ThrowDirection(), this);
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
        float force = _throwPower * _maxThrowForce;
        throwDirection = (forward + upward).normalized * force;
        
        return throwDirection + _rb.linearVelocity;
    }
   
    
    protected void TryAction<T>(Action<T> onFound) where T : class
    {
        Collider[] hits = Physics.OverlapSphere(CatchPoint.position, _sphereRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject || !hit.TryGetComponent(out T component)) continue;
            onFound(component);
            break;
        }
    }
    
    private void TryGrab()
    {
        TryAction<IGrabbable>(grabbable =>
        {
            grabbable.Grabbed(this);
            _grabbed = grabbable;
        });
    }

    public void Reset()
    {
        _grabbed = null;
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
        _originalParent = transform.parent;
        transform.SetParent(controller.CatchPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Rb.isKinematic = true;
    }

    public void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        Debug.Log("Dropped");
        transform.SetParent(_originalParent);

        Rb.isKinematic = false;
        
        if (throwForce != Vector3.zero)
        {
            Movement.FreeMovement = true;
            _rb.AddForce(throwForce, ForceMode.Impulse);
        }

        StartCoroutine(DropRoutine());
        
        if (controller) controller.Reset();
    }

    private IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _ragdoll.RagdollOn();
        Movement.FreeMovement = false;
    }
}
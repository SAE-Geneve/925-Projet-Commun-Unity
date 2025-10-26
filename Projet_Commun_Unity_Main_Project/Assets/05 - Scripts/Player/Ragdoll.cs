using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Action = System.Action;

public class Ragdoll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject playerRig;
    [SerializeField] protected Transform hipsTransform;
    
    [Header("Parameters")]
    [SerializeField] protected float ragdollTime = 3f;
    [SerializeField] private float _ragdollVelocityThreshold = 3f;
    
    public event Action OnRagdoll;
    public event Action<Ragdoll> OnRagdollSelf;

    private Animator _animator;
    private Collider _mainCollider;
    private Rigidbody _mainRigidbody;
    private PlayerInput _playerInput;

    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;
    
    private Coroutine _ragdollCoroutine;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _mainCollider = GetComponent<Collider>();
        _mainRigidbody = GetComponent<Rigidbody>();

        if (_playerInput == null)
            _playerInput = GetComponent<PlayerInput>();

        GetRagdollBits();
        RagdollOff();
    }

    private void GetRagdollBits()
    {
        _ragdollColliders = playerRig.GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = playerRig.GetComponentsInChildren<Rigidbody>();
    }

    public virtual void RagdollOn()
    {
        foreach (var col in _ragdollColliders)
            col.enabled = true;

        foreach (var rb in _ragdollRigidbodies)
        {
            rb.isKinematic = false;
            rb.linearVelocity = _mainRigidbody.linearVelocity;
        }

        _mainRigidbody.isKinematic = true;
        _mainCollider.enabled = false;
        _animator.enabled = false;

        if (_playerInput) _playerInput.currentActionMap.Disable();
        
        OnRagdoll?.Invoke();
        OnRagdollSelf?.Invoke(this);
        
        if (_ragdollCoroutine != null)
            StopCoroutine(_ragdollCoroutine);
        _ragdollCoroutine = StartCoroutine(RagdollTimer());
    }

    protected virtual void RagdollOff()
    {
        if (hipsTransform)
            transform.position = hipsTransform.position;

        foreach (var col in _ragdollColliders)
            col.enabled = false;

        foreach (var rb in _ragdollRigidbodies)
            rb.isKinematic = true;

        _mainRigidbody.isKinematic = false;
        _mainCollider.enabled = true;
        _animator.enabled = true;

        if (_playerInput)
            _playerInput.currentActionMap.Enable();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb) && rb.linearVelocity.magnitude >= _ragdollVelocityThreshold)
            RagdollOn();
    }

    private IEnumerator RagdollTimer()
    {
        yield return new WaitForSeconds(ragdollTime);
        RagdollOff();
    }
}

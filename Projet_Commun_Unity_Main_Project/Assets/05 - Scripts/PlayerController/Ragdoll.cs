using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private float ragdollTime = 3f;
    private Coroutine _ragdollCoroutine;
    
    private Animator _animator;
    private Collider _mainCollider;
    private Rigidbody _mainRigidbody;
    private PlayerInput _playerInput;
    
    [SerializeField] private GameObject playerRig;    
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _mainCollider = GetComponent<Collider>();
        _mainRigidbody = GetComponent<Rigidbody>();
        _playerInput  = GetComponent<PlayerInput>();
        
        GetRagdollBits();
        RagdollOff();
    }

    private void GetRagdollBits()
    {
        _ragdollColliders = playerRig.GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = playerRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollOn()
    {
        foreach (Collider col in _ragdollColliders)
        {
            col.enabled = true;
        }
        
        foreach (Rigidbody rb in _ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
        
        _mainRigidbody.isKinematic = true;
        _mainCollider.enabled = false;
        _animator.enabled = false;
        _playerInput.enabled = false;

        if (_ragdollCoroutine != null)
        {
            StopCoroutine(_ragdollCoroutine);
        }
        
        _ragdollCoroutine = StartCoroutine("RagdollTimer");
    }
    
    public void RagdollOff()
    {
        foreach (Collider col in _ragdollColliders)
        {
            col.enabled = false;
        }
        
        foreach (Rigidbody rb in _ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
        
        _mainRigidbody.isKinematic = false;
        _mainCollider.enabled = true;
        _animator.enabled = true;
        _playerInput.enabled = true;
    }
    
    
    private IEnumerator RagdollTimer()
    {
        yield return new WaitForSeconds(ragdollTime);
        RagdollOff();
    }
}

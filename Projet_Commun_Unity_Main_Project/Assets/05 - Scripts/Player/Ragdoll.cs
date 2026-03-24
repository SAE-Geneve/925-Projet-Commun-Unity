using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ragdoll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject playerRig;
    [SerializeField] protected Transform hipsTransform;
    
    [Header("Parameters")]
    [SerializeField] protected float ragdollTime = 3f;
    [SerializeField] protected float ragdollImmunityDuration = 1.5f; 
    [SerializeField] protected float velocityThreshold = 7f;
    [SerializeField] protected float impulseThreshold = 6f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip ragdollClip;
    [SerializeField] [Range(0f, 1f)] private float ragdollVolume = 1f;
    [SerializeField] private AudioSource audioSource;
    public event Action OnRagdoll;
    public event Action<Ragdoll> OnRagdollSelf;
    
    public bool IsRagdoll { get; private set; }
    public bool IsImmune { get; set; }

    private Animator _animator;
    private CapsuleCollider _mainCollider;
    private Rigidbody _mainRigidbody;
    private PlayerInput _playerInput;

    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;
    
    private Coroutine _ragdollCoroutine;
    private Coroutine _immunityCoroutine;
    private AudioManager _audioManager;

    private Vector3 _playerRigLocalPosition;
    private Quaternion _playerRigLocalRotation;

    private static Transform _rigContainer;

    private static Transform RigContainer
    {
        get
        {
            if (_rigContainer != null) return _rigContainer;
            _rigContainer = new GameObject("ActiveRagdolls").transform;
            _rigContainer.SetParent(GameManager.Instance.transform);
            return _rigContainer;
        }
    }

    protected virtual void Start()
    {
        _audioManager = AudioManager.Instance;
        _animator = GetComponent<Animator>();
        _mainCollider = GetComponent<CapsuleCollider>();
        _mainRigidbody = GetComponent<Rigidbody>();

        if (_playerInput == null)
            _playerInput = GetComponent<PlayerInput>();

        GetRagdollBits();
        _playerRigLocalPosition = playerRig.transform.localPosition;
        _playerRigLocalRotation = playerRig.transform.localRotation;
        RagdollOff();
    }
    
    private void Update()
    {
        if (IsRagdoll && hipsTransform != null)
            transform.position = hipsTransform.position;
    }


    private void GetRagdollBits()
    {
        _ragdollColliders = playerRig.GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = playerRig.GetComponentsInChildren<Rigidbody>();
    }

    public virtual void RagdollOn(bool ignoreImmunity = false)
    {
        if (IsRagdoll || IsImmune && !ignoreImmunity) return;
        
        foreach (var col in _ragdollColliders)
            col.enabled = true;

        foreach (var rb in _ragdollRigidbodies)
        {
            rb.isKinematic = false;
            rb.linearVelocity = _mainRigidbody.linearVelocity;
        }

        if (_playerInput) _playerInput.currentActionMap.Disable();
        
        OnRagdoll?.Invoke();
        OnRagdollSelf?.Invoke(this);
        
        _mainRigidbody.isKinematic = true;
        _mainCollider.isTrigger = true;
        _animator.enabled = false;

        playerRig.transform.SetParent(RigContainer);
        
        if (_ragdollCoroutine != null)
            StopCoroutine(_ragdollCoroutine);
            
        _ragdollCoroutine = StartCoroutine(RagdollTimer());

        IsRagdoll = true;
    }

    protected virtual void RagdollOff()
    {
        playerRig.transform.SetParent(transform);
        playerRig.transform.localPosition = _playerRigLocalPosition;
        playerRig.transform.localRotation = _playerRigLocalRotation;

        foreach (var col in _ragdollColliders)
            col.enabled = false;

        foreach (var rb in _ragdollRigidbodies)
            rb.isKinematic = true;

        _mainRigidbody.isKinematic = false;
        _mainCollider.isTrigger = false;
        _animator.enabled = true;

        if (_playerInput) _playerInput.currentActionMap.Enable();

        IsRagdoll = false;
        
        StartImmunity();
    }
    
    private void StartImmunity()
    {
        if (_immunityCoroutine != null)
            StopCoroutine(_immunityCoroutine);
        _immunityCoroutine = StartCoroutine(ImmunityTimer());
    }
    
    private IEnumerator ImmunityTimer()
    {
        IsImmune = true;
        yield return new WaitForSeconds(ragdollImmunityDuration);
        IsImmune = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsImmune || IsRagdoll) return; 

        float currentRelativeVelocity = other.relativeVelocity.magnitude;
        float currentImpulse = other.impulse.magnitude;
        float strikerVelocity = 0f;

        if (other.rigidbody != null)
        {
            strikerVelocity = other.rigidbody.linearVelocity.magnitude;
        }

        if (currentRelativeVelocity >= velocityThreshold || currentImpulse >= impulseThreshold || strikerVelocity >= velocityThreshold)
        {
            OnRagdolledBy(other.gameObject); 

            RagdollOn();
            
            PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
            
            if (other.gameObject.CompareTag("Player") || gameObject.CompareTag("Player") || player != null)
            {
                if (CameraShakeManager.Instance) 
                    CameraShakeManager.Instance.Shake(0.7f, 0.7f, 0.2f);
            }
        }
    }

    protected virtual void OnRagdolledBy(GameObject striker)
    {
    }

    private IEnumerator RagdollTimer()
    {
        yield return new WaitForSeconds(ragdollTime);
        RagdollOff();
    }
}
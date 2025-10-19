using System.Collections;
using UnityEngine;
using Unity.Behavior;
using Action = System.Action;

public class AIRagdoll : MonoBehaviour
{
    [Header("Ragdoll Settings")]
    [SerializeField] private float ragdollTime = 3f;
    [SerializeField] private GameObject aiRig; 
    [SerializeField] private Transform hipsTransform; 

    [Header("Behavior Graph")]
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;

    [Header("Debug/Test")]
    [SerializeField] private bool testRagdoll = false;

    private Animator _animator;
    private Collider _mainCollider;
    private Rigidbody _mainRigidbody;
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;
    private Coroutine _ragdollCoroutine;

    public bool IsRagdoll { get; private set; }
    public event Action OnRagdoll;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _mainCollider = GetComponent<Collider>();
        _mainRigidbody = GetComponent<Rigidbody>();
        GetRagdollBits();
        RagdollOff();
    }

    private void GetRagdollBits()
    {
        _ragdollColliders = aiRig.GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = aiRig.GetComponentsInChildren<Rigidbody>();
    }

    void Update()
    {
        if (testRagdoll)
        {
            testRagdoll = false;
            RagdollOn();
        }
    }

    public void RagdollOn()
    {
        if (IsRagdoll) return;
        IsRagdoll = true;
        OnRagdoll?.Invoke();

        // Mettre le bool dans le Blackboard à true
        SetVariableInBlackboard(true, "IsRagdoll");

        foreach (Collider col in _ragdollColliders)
            col.enabled = true;

        foreach (Rigidbody rb in _ragdollRigidbodies)
        {
            rb.isKinematic = false;
            rb.linearVelocity = _mainRigidbody.linearVelocity;
        }

        _mainRigidbody.isKinematic = true;
        _mainCollider.enabled = false;
        _animator.enabled = false;

        if (_ragdollCoroutine != null)
            StopCoroutine(_ragdollCoroutine);
        _ragdollCoroutine = StartCoroutine(RagdollTimer());
    }

    public void RagdollOff()
    {
        IsRagdoll = false;

        transform.position = hipsTransform.position;

        foreach (Collider col in _ragdollColliders)
            col.enabled = false;

        foreach (Rigidbody rb in _ragdollRigidbodies)
            rb.isKinematic = true;

        _mainRigidbody.isKinematic = false;
        _mainCollider.enabled = true;
        _animator.enabled = true;

        // Mettre le bool dans le Blackboard à false
        SetVariableInBlackboard(false, "IsRagdoll");
    }

    private IEnumerator RagdollTimer()
    {
        yield return new WaitForSeconds(ragdollTime);
        RagdollOff();
    }

    private void SetVariableInBlackboard<T>(T value, string variableName)
    {
        if (behaviorGraphAgent == null) return;

        if (behaviorGraphAgent.GetVariable<T>(variableName, out var variable))
        {
            variable.Value = value;
        }
        else
        {
            Debug.LogWarning($"Variable '{variableName}' introuvable dans le Blackboard !");
        }
    }

}

using UnityEngine;
using Unity.Behavior;

public class AIRagdoll : Ragdoll
{
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private GameObject bHips;
    [SerializeField] private bool isRagdollable = true;

    private bool IsRagdollState { get; set; }
    private float _lastRagdollOffTime;
    private Rigidbody _mainRb;

    protected override void Start()
    {
        base.Start();
        _mainRb = GetComponent<Rigidbody>();
    }

    public override void RagdollOn()
    {
        if (Time.time < _lastRagdollOffTime + 1.0f) return;
        if (!isRagdollable || IsRagdollState) return;

        Vector3 currentVelocity = Vector3.zero;
        if (_mainRb != null) currentVelocity = _mainRb.linearVelocity;

        IsRagdollState = true;
        bHips.SetActive(true);

        Collider[] cols = bHips.GetComponentsInChildren<Collider>(true);
        foreach (var col in cols) col.enabled = true;

        Rigidbody[] rbs = bHips.GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.linearVelocity = currentVelocity;
        }

        base.RagdollOn();
        SetVariableInBlackboard(true, "IsRagdoll");
    }

    protected override void RagdollOff()
    {
        if (!IsRagdollState) return;

        Rigidbody[] rbs = bHips.GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in rbs)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider[] cols = bHips.GetComponentsInChildren<Collider>(true);
        foreach (var col in cols) col.enabled = false;

        bHips.SetActive(false);
        IsRagdollState = false;
        _lastRagdollOffTime = Time.time;

        base.RagdollOff();

        if (_mainRb)
        {
            _mainRb.linearVelocity = Vector3.zero;
            _mainRb.angularVelocity = Vector3.zero;
        }

        SetVariableInBlackboard(false, "IsRagdoll");
    }

    private void SetVariableInBlackboard<T>(T value, string variableName)
    {
        if (!behaviorGraphAgent) return;
        if (behaviorGraphAgent.GetVariable<T>(variableName, out var variable))
            variable.Value = value;
    }
}
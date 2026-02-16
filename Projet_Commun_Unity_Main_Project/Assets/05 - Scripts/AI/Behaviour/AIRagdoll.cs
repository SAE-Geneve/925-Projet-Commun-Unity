using UnityEngine;
using Unity.Behavior;

public class AIRagdoll : Ragdoll
{
    [Header("Behavior Graph")]
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private GameObject bHips;

    [Header("Ragdoll Settings")]
    [SerializeField] private bool isRagdollable = true;

    private bool IsRagdollState { get; set; }
    private float _lastRagdollOffTime; 

    public override void RagdollOn()
    {
        if (Time.time < _lastRagdollOffTime + 0.5f) return;

        if (!isRagdollable || IsRagdollState) return;

        Debug.LogWarning("REACTIVATE");
        bHips.SetActive(true);
        IsRagdollState = true;

        base.RagdollOn();
        SetVariableInBlackboard(true, "IsRagdoll");
    }

    protected override void RagdollOff()
    {
        Debug.LogWarning("DESACTIVATE");
        
        if (!IsRagdollState) return;
        
        bHips.SetActive(false);
        IsRagdollState = false;
        
        _lastRagdollOffTime = Time.time;

        base.RagdollOff();
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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
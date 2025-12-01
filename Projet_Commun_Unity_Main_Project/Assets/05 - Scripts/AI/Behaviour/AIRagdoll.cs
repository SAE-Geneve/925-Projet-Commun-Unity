using UnityEngine;
using Unity.Behavior;

public class AIRagdoll : Ragdoll
{
    [Header("Behavior Graph")]
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private GameObject bHips;

    private bool IsRagdollState { get; set; }

    public override void RagdollOn()
    {
        bHips.SetActive(true);

        if (IsRagdollState) return;
        IsRagdollState = true;

        base.RagdollOn();
        SetVariableInBlackboard(true, "IsRagdoll");
    }

    protected override void RagdollOff()
    {
        bHips.SetActive(false);

        if (!IsRagdollState) return;
        IsRagdollState = false;

        base.RagdollOff();
        SetVariableInBlackboard(false, "IsRagdoll");
    }

    private void SetVariableInBlackboard<T>(T value, string variableName)
    {
        if (!behaviorGraphAgent) return;

        if (behaviorGraphAgent.GetVariable<T>(variableName, out var variable))
            variable.Value = value;
        else
            Debug.LogWarning($"Variable '{variableName}' introuvable dans le Blackboard !");
    }
}
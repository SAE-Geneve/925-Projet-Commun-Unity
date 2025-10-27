using UnityEngine;
using Unity.Behavior;

public class AIRagdollTest : Ragdoll
{
    [Header("Behavior Graph")]
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;

    [Header("Debug/Test")]
    [SerializeField] private bool testRagdoll = false;

    public bool IsRagdollState { get; private set; }

    void Update()
    {
        if (testRagdoll)
        {
            testRagdoll = false;
            RagdollOn();
        }
    }

    public override void RagdollOn()
    {
        if (IsRagdollState) return;
        IsRagdollState = true;

        base.RagdollOn();
        
        SetVariableInBlackboard(true, "IsRagdoll");
    }

    protected override void RagdollOff()
    {
        if (!IsRagdollState) return;
        IsRagdollState = false;

        base.RagdollOff();
        
        SetVariableInBlackboard(false, "IsRagdoll");
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
using Unity.Behavior;
using UnityEngine;

public class BlackboardTask : GameTask
{
    [Header("Blackboard")] [SerializeField]
    private BehaviorGraphAgent agent;

    [Header("Variable")] [SerializeField] private string variableName = "IsHappy";
    [SerializeField] private bool expectedValue = true;

    private BlackboardVariable<bool> variableContainer;

    protected override void Start()
    {
        base.Start();

        if (!agent.GetVariable(variableName, out variableContainer))
        {
            Debug.LogWarning($"Variable '{variableName}' not found in Blackboard.");
            Failed();
        }
    }

    private void Update()
    {
        if (Done) return;

        bool currentValue = variableContainer.Value;

        if (currentValue == expectedValue)
        {
            Succeed();
        }
    }
}
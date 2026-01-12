using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Move To Target (AIMovement)",
    story: "[self] moves physically to [target]",
    category: "Action/Navigation",
    id: "ai_movetarget_aimovement")]
public partial class MoveToTargetAIMovementAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private AIMovement aiMove;

    protected override Status OnStart()
    {
        if (Self?.Value == null || Target?.Value == null)
            return Status.Failure;

        aiMove = Self.Value.GetComponent<AIMovement>();
        if (aiMove == null)
            return Status.Failure;

        aiMove.SetDestination(Target.Value.transform.position);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (aiMove == null)
            return Status.Failure;

        if (aiMove.HasReachedDestination())
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }
}
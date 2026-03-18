using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Move To Target In Fixed Time (AIMovement)",
    story: "[self] moves to [target] in exactly [duration] seconds",
    category: "Action/Navigation",
    id: "ai_movetarget_fixedtime_aimovement")]
public partial class MoveToTargetFixedTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Duration;

    private AIMovement _aiMove;
    private float _originalSpeed;

    protected override Status OnStart()
    {
        if (Self?.Value == null || Target?.Value == null)
            return Status.Failure;

        _aiMove = Self.Value.GetComponent<AIMovement>();
        if (_aiMove == null)
            return Status.Failure;

        float distance = Vector3.Distance(
            Self.Value.transform.position,
            Target.Value.transform.position
        );

        float duration = (Duration?.Value > 0f) ? Duration.Value : 1f;
        
        _originalSpeed = _aiMove.speed;
        _aiMove.speed = distance / duration;

        _aiMove.SetDestination(Target.Value.transform.position);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_aiMove == null)
            return Status.Failure;

        if (_aiMove.HasReachedDestination())
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (_aiMove != null)
            _aiMove.speed = _originalSpeed;

        _aiMove?.Stop();
    }
}
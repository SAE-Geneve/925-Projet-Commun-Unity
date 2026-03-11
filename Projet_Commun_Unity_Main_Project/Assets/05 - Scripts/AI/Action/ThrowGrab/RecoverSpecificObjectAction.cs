using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Recover Specific Object", story: "If [Self] lost [TargetObject], recover it and update [IsLostBool]", category: "Action", id: "ai_recover_specific_object")]
public partial class RecoverSpecificObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetObject;
    [SerializeReference] public BlackboardVariable<bool> IsLostBool;

    private AIMovement aiMove; 
    private Controller _controller; 
    private Transform selfTransform;
    
    private enum Phase { Moving, Done }
    private Phase phase;

    protected override Status OnStart()
    {
        if (Self?.Value == null || TargetObject?.Value == null) return Status.Failure;
        
        aiMove = Self.Value.GetComponent<AIMovement>();
        _controller = Self.Value.GetComponent<Controller>();
        selfTransform = Self.Value.transform;

        if (aiMove == null || _controller == null) return Status.Failure;

        phase = Phase.Moving;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (phase == Phase.Done) return Status.Success;

        if (phase == Phase.Moving)
        {
            MoveToTarget();
        }

        return Status.Running;
    }

    private void MoveToTarget()
    {
        aiMove.SetDestination(TargetObject.Value.transform.position);

        float distance = Vector3.Distance(selfTransform.position, TargetObject.Value.transform.position);
        
        if (distance <= 1.5f)
        {
            if (TargetObject.Value.TryGetComponent(out IGrabbable grabbable))
            {
                grabbable.Grabbed(_controller);
            }
            
            if (TargetObject.Value.TryGetComponent(out Prop prop))
            {
                _controller.SetGrabbedProp(prop); 
            }
            
            if (IsLostBool != null) IsLostBool.Value = false;

            aiMove.Stop();
            phase = Phase.Done; 
        }
    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }
}
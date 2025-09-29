using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfHitPlayer", story: "[self] hit [player]", category: "Action", id: "5d0d270614a901b06627ec9175fb0b4e")]
public partial class SelfHitPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self == null || Self.Value == null || Player == null || Player.Value == null)
            return Status.Failure;
        
        Collider selfCollider = Self.Value.GetComponent<Collider>();
        Collider playerCollider = Player.Value.GetComponent<Collider>();

        if (selfCollider == null || playerCollider == null)
            return Status.Failure;
        
        if (selfCollider.bounds.Intersects(playerCollider.bounds))
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}
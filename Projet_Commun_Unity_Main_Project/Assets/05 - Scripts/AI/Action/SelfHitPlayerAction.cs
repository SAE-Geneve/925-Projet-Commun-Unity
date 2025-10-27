using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfHitPlayer", story: "[self] touches a player", category: "Action", id: "self_hit_player")]
public partial class SelfHitPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self?.Value == null)
            return Status.Failure;
        
        Collider selfCol = Self.Value.GetComponent<Collider>();
        if (selfCol == null)
            return Status.Failure;
        
        Collider[] hits = Physics.OverlapBox(
            selfCol.bounds.center,
            selfCol.bounds.extents,
            Quaternion.identity,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                return Status.Success;
            }
        }

        return Status.Running;
    }
}
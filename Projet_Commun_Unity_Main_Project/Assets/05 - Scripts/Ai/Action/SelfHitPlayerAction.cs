using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfHitPlayer", story: "[self] hit player", category: "Action", id: "5d0d270614a901b06627ec9175fb0b4e")]
public partial class SelfHitPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self == null || Self.Value == null)
            return Status.Failure;

        Collider selfCollider = Self.Value.GetComponent<Collider>();
        if (selfCollider == null)
            return Status.Failure;
        
        Collider[] hits = Physics.OverlapBox(
            selfCollider.bounds.center,
            selfCollider.bounds.extents,
            Self.Value.transform.rotation
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
                return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd() { }
}
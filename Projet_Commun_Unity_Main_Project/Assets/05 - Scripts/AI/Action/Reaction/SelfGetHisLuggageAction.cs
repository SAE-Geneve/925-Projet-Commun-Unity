using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Self get his luggage", story: "[self] get his [luggage]", category: "Action", id: "f6b8f5f36e14609dea15563c8b444c9a")]
public partial class SelfGetHisLuggageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Luggage;

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Luggage?.Value == null)
            return Status.Failure;

        Collider selfCol = Self.Value.GetComponent<Collider>();
        Collider luggageCol = Luggage.Value.GetComponent<Collider>();
        if (selfCol == null || luggageCol == null)
            return Status.Failure;
        
        Collider[] hits = Physics.OverlapBox(
            selfCol.bounds.center,
            selfCol.bounds.extents,
            Self.Value.transform.rotation
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value)
                continue;

            if (hit.gameObject == Luggage.Value)
            {
                return Status.Success;
            }
        }

        return Status.Running;
    }
    
    protected override void OnEnd() { }
}
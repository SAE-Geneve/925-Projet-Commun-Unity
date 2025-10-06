using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Self get his luggage", story: "[self] get his [luggage]", category: "Action", id: "f6b8f5f36e14609dea15563c8b444c9a")]
public partial class SelfGetHisLuggageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Luggage;


    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Luggage?.Value == null)
            return Status.Failure;

        // Vérifie que les deux GameObjects ont des colliders
        Collider selfCol = Self.Value.GetComponent<Collider>();
        Collider luggageCol = Luggage.Value.GetComponent<Collider>();

        if (selfCol == null || luggageCol == null)
            return Status.Failure; // Impossible de détecter sans colliders

        // Vérifie si Self touche Luggage
        if (selfCol.bounds.Intersects(luggageCol.bounds))
        {
           

            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // Pas d'action spécifique à la fin
    }
}


using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "ThrowObject",
    story: "[Self] throws the grabbed object at a nearby player if any, otherwise randomly",
    category: "Action",
    id: "ai_throw_object_combined")]
public partial class ThrowObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> ThrowRange = new BlackboardVariable<float>() { Value = 5f };
    [SerializeReference] public BlackboardVariable<float> ThrowForce = new BlackboardVariable<float>() { Value = 10f };
    [SerializeReference] public BlackboardVariable<IGrabbable> GrabbedObject;

    private Controller _controller;
    private Transform selfTransform;

    protected override Status OnStart()
    {
        if (Self?.Value == null || GrabbedObject?.Value == null)
            return Status.Failure;

        _controller = Self.Value.GetComponent<Controller>();
        selfTransform = Self.Value.transform;

        if (_controller == null)
            return Status.Failure;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (GrabbedObject?.Value == null)
            return Status.Failure;

        Vector3 throwDirection = Vector3.zero;

        // Cherche le joueur le plus proche
        Collider[] hits = Physics.OverlapSphere(selfTransform.position, ThrowRange.Value);
        Transform closestPlayer = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                float dist = Vector3.Distance(selfTransform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestPlayer = hit.transform;
                    closestDist = dist;
                }
            }
        }

        if (closestPlayer != null)
        {
            throwDirection = (closestPlayer.position - selfTransform.position).normalized;
        }
        else
        {
            // Si aucun joueur → lance aléatoirement
            Vector2 random2D = UnityEngine.Random.insideUnitCircle.normalized;
            throwDirection = new Vector3(random2D.x, 0f, random2D.y);
        }

        // Lance l'objet
        GrabbedObject.Value.Dropped(throwDirection * ThrowForce.Value, _controller);

        return Status.Success;
    }
}

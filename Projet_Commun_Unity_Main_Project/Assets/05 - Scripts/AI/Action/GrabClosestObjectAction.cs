using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GrabClosestObject", story: "[Self] grabs the closest object", category: "Action", id: "ai_grab_closest")]
public partial class GrabClosestObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private AIMovementTest aiMove;
    private Controller _controller;
    private Transform selfTransform;
    public IGrabbable GrabbedObject { get; private set; }

    protected override Status OnStart()
    {
        if (Self?.Value == null)
            return Status.Failure;

        aiMove = Self.Value.GetComponent<AIMovementTest>();
        _controller = Self.Value.GetComponent<Controller>();
        selfTransform = Self.Value.transform;

        if (aiMove == null || _controller == null)
            return Status.Failure;

        GrabbedObject = null;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(selfTransform.position, 3f);
        IGrabbable closest = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IGrabbable grabbable))
            {
                float dist = Vector3.Distance(selfTransform.position, (grabbable as MonoBehaviour).transform.position);
                if (dist < closestDist)
                {
                    closest = grabbable;
                    closestDist = dist;
                }
            }
        }

        if (closest != null)
        {
            GrabbedObject = closest;
            closest.Grabbed(_controller);
            return Status.Success;
        }

        return Status.Failure;
    }
}
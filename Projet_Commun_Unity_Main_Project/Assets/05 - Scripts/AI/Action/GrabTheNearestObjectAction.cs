using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GrabTheNearestObject", story: "[self] grab an object and assign [GrabbedObject]", category: "Action", id: "ai_grabandthrow_random")]
public partial class GrabTheNearestObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> GrabbedObject;
    private AIMovementTest aiMove;
    private Controller _controller;
    private Transform selfTransform;
    private IGrabbable targetGrabbable;

    private enum Phase { Searching, Moving, Done }
    private Phase phase;

    protected override Status OnStart()
    {
        if (Self?.Value == null)
            return Status.Failure;

        aiMove = Self.Value.GetComponent<AIMovementTest>();
        _controller = Self.Value.GetComponent<Controller>();
        selfTransform = Self.Value.transform;

        if (aiMove == null || _controller == null)
            return Status.Failure;

        phase = Phase.Searching;
        targetGrabbable = null;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (phase == Phase.Done)
            return Status.Success;

        switch (phase)
        {
            case Phase.Searching:
                SearchForGrabbable();
                break;

            case Phase.Moving:
                MoveToTarget();
                break;
        }

        return Status.Running;
    }

    private void SearchForGrabbable()
    {
        Collider[] hits = Physics.OverlapSphere(selfTransform.position, 15f);
    
        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value)
            {
                continue;
            }
    
            if (!hit.CompareTag("AIProp"))
            {
                continue;
            }
    
            if (hit.TryGetComponent(out IGrabbable grabbable))
            {
                targetGrabbable = grabbable;
                aiMove.SetDestination(hit.transform.position);
                GrabbedObject.Value = hit.gameObject;
                phase = Phase.Moving;
                return;
            }
        }
        phase = Phase.Done;
    }
    private void MoveToTarget()
    {
        if (targetGrabbable == null)
        {
            phase = Phase.Done;
            return;
        }

        float distance =
            Vector3.Distance(selfTransform.position, (targetGrabbable as MonoBehaviour).transform.position);

        if (distance <= 1.5f)
        {
            targetGrabbable.Grabbed(_controller);
            aiMove.Stop();
            phase = Phase.Done;
        }

    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }
}

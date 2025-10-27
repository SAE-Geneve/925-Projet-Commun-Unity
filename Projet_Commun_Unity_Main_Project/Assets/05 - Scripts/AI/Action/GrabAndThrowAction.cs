using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GrabAndThrow", story: "[self] grab an object and throw it randomly", category: "Action", id: "ai_grabandthrow_random")]
public partial class GrabAndThrowAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private AIMovementTest aiMove;
    private Controller _controller;
    private Transform selfTransform;
    private IGrabbable targetGrabbable;

    private enum Phase { Searching, Moving, Turning, Throwing, Done }
    private Phase phase;

    private Vector3 randomDirection;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float turnDuration = .8f;
    private float turnTimer = 0f;

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

            case Phase.Turning:
                HandleTurning();
                break;

            case Phase.Throwing:
                HandleThrowing();
                break;
        }

        return Status.Running;
    }

    private void SearchForGrabbable()
    {
        Collider[] hits = Physics.OverlapSphere(selfTransform.position, 3f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IGrabbable grabbable))
            {
                targetGrabbable = grabbable;
                aiMove.SetDestination(hit.transform.position);
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

        float distance = Vector3.Distance(selfTransform.position, (targetGrabbable as MonoBehaviour).transform.position);

        if (distance <= 1.5f)
        {
            targetGrabbable.Grabbed(_controller);
            aiMove.Stop();
            
            Vector2 random2D = UnityEngine.Random.insideUnitCircle.normalized;
            randomDirection = new Vector3(random2D.x, 0f, random2D.y);
            
            startRotation = selfTransform.rotation;
            targetRotation = Quaternion.LookRotation(randomDirection);
            turnTimer = 0f;

            phase = Phase.Turning;
        }
    }

    private void HandleTurning()
    {
        turnTimer += Time.deltaTime;

        float t = Mathf.Clamp01(turnTimer / turnDuration);
        selfTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            phase = Phase.Throwing;
        }
    }

    private void HandleThrowing()
    {
        _controller.InteractableGrabbed = null;
        targetGrabbable.Dropped(selfTransform.forward * 10f, _controller);

        phase = Phase.Done;
    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }
}

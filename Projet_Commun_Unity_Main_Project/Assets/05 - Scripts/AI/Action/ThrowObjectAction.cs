using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "ThrowObject",
    story: "[Self] throws the grabbed object at a nearby player if any, otherwise randomly with turning",
    category: "Action",
    id: "ai_throw_object_combined")]
public partial class ThrowObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> GrabbedObjectGO;
    [SerializeReference] public BlackboardVariable<float> ThrowRange = new BlackboardVariable<float>() { Value = 5f };
    [SerializeReference] public BlackboardVariable<float> ThrowForce = new BlackboardVariable<float>() { Value = 10f };
    [SerializeReference] public BlackboardVariable<float> TurnDuration = new BlackboardVariable<float>() { Value = 0.8f };

    private Controller _controller;
    private Transform selfTransform;
    private IGrabbable grabbedObject;

    private enum Phase { Turning, Throwing, Done }
    private Phase phase;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float turnTimer = 0f;

    protected override Status OnStart()
    {
        if (Self?.Value == null || GrabbedObjectGO?.Value == null)
            return Status.Failure;

        _controller = Self.Value.GetComponent<Controller>();
        selfTransform = Self.Value.transform;

        grabbedObject = GrabbedObjectGO.Value.GetComponent<IGrabbable>();
        if (_controller == null || grabbedObject == null)
            return Status.Failure;

        // Détermine la direction de lancement
        Vector3 throwDir = Vector3.zero;
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
            throwDir = (closestPlayer.position - selfTransform.position).normalized;
        else
        {
            Vector2 random2D = UnityEngine.Random.insideUnitCircle.normalized;
            throwDir = new Vector3(random2D.x, 0f, random2D.y);
        }

        // Initialise la rotation vers la direction
        startRotation = selfTransform.rotation;
        targetRotation = Quaternion.LookRotation(throwDir);
        turnTimer = 0f;
        phase = Phase.Turning;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (phase == Phase.Done) return Status.Success;

        turnTimer += Time.deltaTime;
        float t = Mathf.Clamp01(turnTimer / TurnDuration.Value);
        selfTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        if (t >= 1f && phase == Phase.Turning)
        {
            grabbedObject.Dropped(selfTransform.forward * ThrowForce.Value, _controller);
            phase = Phase.Done;
        }

        return Status.Running;
    }
}

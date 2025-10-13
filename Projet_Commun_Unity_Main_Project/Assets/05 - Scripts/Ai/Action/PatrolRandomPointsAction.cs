using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolRandomPoints", story: "[Agent] moves randomly between [points]", category: "Action", id: "05c3c1c6df4cb824873b7859a3542306")]
public partial class PatrolRandomPointsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Points;
    [SerializeReference] public BlackboardVariable<float> StopDistance = new(0.5f);
    [SerializeReference] public BlackboardVariable<float> WaitTime = new(1f);

    private CharacterMovement movement;
    private GameObject currentTarget;
    private NavMeshPath path;
    private int currentCorner = 0;
    private float waitTimer = 0f;

    protected override Status OnStart()
    {
        if (Agent?.Value == null || Points?.Value == null || Points.Value.Count == 0)
            return Status.Failure;

        movement = Agent.Value.GetComponent<CharacterMovement>();
        if (movement == null) return Status.Failure;

        path = new NavMeshPath();
        PickNextTarget();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent?.Value == null || Points?.Value == null || Points.Value.Count == 0 || movement == null)
            return Status.Failure;

        // Attente si on a atteint le waypoint précédent
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            movement.SetMovement(Vector2.zero);
            return Status.Running;
        }

        Vector3 agentPos = Agent.Value.transform.position;

        // Calculer le chemin vers le waypoint si nécessaire
        if (path.corners.Length == 0 || currentCorner >= path.corners.Length)
        {
            if (!NavMesh.CalculatePath(agentPos, currentTarget.transform.position, NavMesh.AllAreas, path))
            {
                return Status.Failure;
            }
            currentCorner = 0;
        }

        Vector3 targetPos = path.corners[currentCorner];
        Vector3 dir = targetPos - agentPos;
        dir.y = 0f;

        if (dir.magnitude <= StopDistance.Value)
        {
            currentCorner++;
            if (currentCorner >= path.corners.Length)
            {
                waitTimer = WaitTime.Value;
                PickNextTarget();
                movement.SetMovement(Vector2.zero);
            }
        }
        else
        {
            dir.Normalize();
            movement.SetMovement(new Vector2(dir.x, dir.z));
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        movement?.SetMovement(Vector2.zero);
    }

    private void PickNextTarget()
    {
        if (Points.Value.Count == 0) return;
        int index = UnityEngine.Random.Range(0, Points.Value.Count);
        currentTarget = Points.Value[index];
        path.ClearCorners();
        currentCorner = 0;
    }
}


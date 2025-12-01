using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolRandomPoints", story: "[Agent] moves randomly to a point", category: "Action", id: "ai_patrolrandompoints_oneway")]
public partial class PatrolRandomPointsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Points;

    private AIMovement aiMove;
    private GameObject currentTarget;

    protected override Status OnStart()
    {
        if (Agent?.Value == null || Points?.Value == null || Points.Value.Count == 0)
            return Status.Failure;

        aiMove = Agent.Value.GetComponent<AIMovement>();
        if (aiMove == null) return Status.Failure;

        PickNextTarget();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (aiMove == null || currentTarget == null)
            return Status.Failure;

        // Vérifie si l'IA a atteint le point
        if (aiMove.HasReachedDestination())
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }

    private void PickNextTarget()
    {
        if (Points.Value.Count == 0) return;

        int index = UnityEngine.Random.Range(0, Points.Value.Count);
        currentTarget = Points.Value[index];
        aiMove.SetDestination(currentTarget.transform.position);
    }
}
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToRandomPoint", story: "[self] move to a random point physically", category: "Action", id: "ai_gotorandompoint")]
public partial class GoToRandomPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    public float RandomPointRadius = 10f;

    private AIMovementTest aiMove;
    private Vector3 destination;

    protected override Status OnStart()
    {
        if (Self?.Value == null) return Status.Failure;

        aiMove = Self.Value.GetComponent<AIMovementTest>();
        if (aiMove == null) return Status.Failure;

        if (!GetRandomPointOnNavMesh(Self.Value.transform.position, RandomPointRadius, out destination))
            return Status.Failure;
        
        aiMove.SetSpeed(6f);
        aiMove.SetDestination(destination);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (aiMove == null) return Status.Failure;

        if (aiMove.HasReachedDestination())
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
        aiMove.SetSpeed(3.5f);
        aiMove?.Stop();
    }

    private bool GetRandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = center;
        return false;
    }
}
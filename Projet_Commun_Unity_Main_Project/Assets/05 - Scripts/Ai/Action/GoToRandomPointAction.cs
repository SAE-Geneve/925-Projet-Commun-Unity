using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToRandomPoint", story: "[self] go to a random point", category: "Action", id: "00cafe8c77ad2c3686b190ee8aa85c7d")]
public partial class GoToRandomPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    public float StopDistance = 0.5f;
    public float RandomPointRadius = 10f;

    private NavMeshAgent agent;
    private bool targetSet = false;

    protected override Status OnStart()
    {
        if (Self == null || Self.Value == null)
            return Status.Failure;
        
        agent = Self.Value.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = Self.Value.AddComponent<NavMeshAgent>();
   
        }
        agent.speed = 6.5f;
        Vector3 randomPoint;
        if (GetRandomPointOnNavMesh(Self.Value.transform.position, RandomPointRadius, out randomPoint))
        {
            agent.SetDestination(randomPoint);
            targetSet = true;
            return Status.Running;
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (Self == null || Self.Value == null || agent == null || !targetSet)
            return Status.Failure;

        if (!agent.pathPending && agent.remainingDistance <= StopDistance){
            agent.speed = 3.5f;
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        targetSet = false;
        if (agent != null)
            agent.ResetPath();
    }

    private bool GetRandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = center;
        return false;
    }
}

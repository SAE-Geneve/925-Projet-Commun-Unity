using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToRandomPoint", story: "[self] move to a random point physically", category: "Action", id: "hybrid_gotorandompoint_sync_debug")]
public partial class GoToRandomPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    public float StopDistance = 0f;
    public float RandomPointRadius = 10f;

    private CharacterMovement movement;
    private Vector3 destination;
    private NavMeshPath path;
    private int currentCorner = 0;
    private bool targetSet = false;

    protected override Status OnStart()
    {
        if (Self?.Value == null) return Status.Failure;

        movement = Self.Value.GetComponent<CharacterMovement>();
        if (movement == null) return Status.Failure;
        movement.SetSpeed(6f);
        path = new NavMeshPath();
        
        if (GetRandomPointOnNavMesh(Self.Value.transform.position, RandomPointRadius, out destination))
        {
            if (!NavMesh.CalculatePath(Self.Value.transform.position, destination, NavMesh.AllAreas, path))
                return Status.Failure;

            currentCorner = 0;
            targetSet = true;
            return Status.Running;
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (!targetSet || Self?.Value == null || movement == null) return Status.Failure;

        Vector3 agentPos = Self.Value.transform.position;
        
        if (Vector3.Distance(agentPos, destination) <= StopDistance)
        {
            movement.SetMovement(Vector2.zero);
            targetSet = false;
            return Status.Success;
        }

        if (path == null || path.corners.Length == 0) return Status.Running;

        Vector3 nextCorner = path.corners[Mathf.Min(currentCorner, path.corners.Length - 1)];
        Vector3 dir = nextCorner - agentPos;
        dir.y = 0f;

        // Debug : visualiser le chemin
        Debug.DrawLine(agentPos, nextCorner, Color.green);

        if (dir.magnitude <= StopDistance)
        {
            currentCorner++;
            
            if (currentCorner >= path.corners.Length)
            {
                movement.SetMovement(Vector2.zero);
                targetSet = false;
                return Status.Success;
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
        movement.SetSpeed(3.5f);
        movement?.SetMovement(Vector2.zero);
        targetSet = false;
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

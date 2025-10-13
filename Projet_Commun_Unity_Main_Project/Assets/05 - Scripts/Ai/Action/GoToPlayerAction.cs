using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToPlayer", story: "[self] move toward [player] physically", category: "Action", id: "ai_gotoplayer")]
public partial class GoToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    public float StopDistance = 1.5f;
    public float RecalculateDistance = 1f;

    private AIMovement aiMove;
    private Vector3 lastDestination;

    protected override Status OnStart()
    {
        if (Self?.Value == null || Player?.Value == null)
            return Status.Failure;

        aiMove = Self.Value.GetComponent<AIMovement>();
        if (aiMove == null)
            return Status.Failure;

        UpdateDestination();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Player?.Value == null || aiMove == null)
            return Status.Failure;

        float distanceToPlayer = Vector3.Distance(Self.Value.transform.position, Player.Value.transform.position);
        if (distanceToPlayer <= StopDistance)
        {
            aiMove.Stop();
            return Status.Success;
        }
        
        if (Vector3.Distance(lastDestination, Player.Value.transform.position) > RecalculateDistance)
        {
            UpdateDestination();
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        aiMove?.Stop();
    }

    private void UpdateDestination()
    {
        if (NavMesh.SamplePosition(Player.Value.transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            lastDestination = hit.position;
            aiMove.SetDestination(lastDestination);
        }
    }
}

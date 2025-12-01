using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToPlayer", story: "[self] move toward closest player physically", category: "Action", id: "ai_gotoplayer")]
public partial class GoToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    public float StopDistance = 1.5f;
    public float RecalculateDistance = 1f;

    private Transform _player;

    private AIMovement _aiMovement;
    private Vector3 lastDestination;

    protected override Status OnStart()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aiMovement = Self.Value.GetComponent<AIMovement>();
        
        if (AnyNull()) return Status.Failure;

        UpdateDestination();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (AnyNull()) return Status.Failure;

        float distanceToPlayer = Vector3.Distance(Self.Value.transform.position, _player.transform.position);
        if (distanceToPlayer <= StopDistance)
        {
            _aiMovement.Stop();
            return Status.Success;
        }
        
        if (Vector3.Distance(lastDestination, _player.position) > RecalculateDistance)
            UpdateDestination();

        return Status.Running;
    }

    protected override void OnEnd() => _aiMovement?.Stop();
    

    private void UpdateDestination()
    {
        if (NavMesh.SamplePosition(_player.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            lastDestination = hit.position;
            _aiMovement.SetDestination(lastDestination);
        }
    }

    private bool AnyNull() => Self?.Value == null || !_player || !_aiMovement;
}

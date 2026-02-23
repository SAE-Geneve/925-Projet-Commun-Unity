using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SearchAction", story: "[Self] patrols in [WanderRadius] and searches for Player within [DetectionRadius]", category: "Action", id: "9772e1cfe5bc6783b2c11be8ca4c386b")]
public partial class SearchAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> WanderRadius = new BlackboardVariable<float>(15f);
    [SerializeReference] public BlackboardVariable<float> DetectionRadius = new BlackboardVariable<float>(7f);
    [SerializeReference] public BlackboardVariable<string> PlayerTag = new BlackboardVariable<string>("Player");
    [SerializeReference] public BlackboardVariable<string> FloorTag = new BlackboardVariable<string>("GameFloor");
    
    [SerializeReference] public BlackboardVariable<float> WaitTime = new BlackboardVariable<float>(1f); 
    
    [SerializeReference] public BlackboardVariable<GameObject> FoundPlayer;

    private AIMovement _aiMove;
    
    private bool _isWaiting;
    private float _waitTimer;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;

        _aiMove = Self.Value.GetComponent<AIMovement>();
        if (_aiMove == null) return Status.Failure;

        _aiMove.SetSpeed(3.5f);
        
        _isWaiting = false;
        SetNewRandomDestination();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_aiMove == null) return Status.Failure;
        
        Collider[] hits = Physics.OverlapSphere(Self.Value.transform.position, DetectionRadius.Value);
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(PlayerTag.Value))
            {
                float dist = Vector3.Distance(Self.Value.transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPlayer = hit.gameObject;
                }
            }
        }

        if (closestPlayer != null)
        {
            FoundPlayer.Value = closestPlayer;
            return Status.Success; 
        }
        
        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                SetNewRandomDestination();
            }
        }
        else if (_aiMove.HasReachedDestination())
        {
            _isWaiting = true;
            _waitTimer = WaitTime.Value; 
        }

        return Status.Running;
    }

    private void SetNewRandomDestination()
    {
        if (GetRandomPointOnNavMesh(Self.Value.transform.position, WanderRadius.Value, out Vector3 result))
        {
            _aiMove.SetDestination(result);
        }
    }

    private bool GetRandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 random2D = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 rayStartPoint = center + new Vector3(random2D.x, 5f, random2D.y);

            if (Physics.Raycast(rayStartPoint, Vector3.down, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag(FloorTag.Value))
                {
                    if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out UnityEngine.AI.NavMeshHit navHit, 2f, UnityEngine.AI.NavMesh.AllAreas))
                    {
                        result = navHit.position;
                        return true;
                    }
                }
            }
        }
        result = center;
        return false;
    }

    protected override void OnEnd()
    {
        _aiMove?.Stop();
    }
}
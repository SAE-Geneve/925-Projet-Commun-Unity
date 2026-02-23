using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cooldown", story: "[Self] wanders randomly for [CooldownTime] seconds within [WanderRadius]", category: "Action", id: "c319987640a470ed46248f5e88f40b4c")]
public partial class CooldownAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> CooldownTime = new BlackboardVariable<float>(5f);
    [SerializeReference] public BlackboardVariable<float> WanderRadius = new BlackboardVariable<float>(10f);
    [SerializeReference] public BlackboardVariable<string> FloorTag = new BlackboardVariable<string>("GameFloor");
    
    [SerializeReference] public BlackboardVariable<float> WaitTime = new BlackboardVariable<float>(1f);

    private AIMovement _aiMove;
    private float _timer;
    
    private bool _isWaiting;
    private float _waitTimer;

    protected override Status OnStart()
    {
        if (Self.Value == null) return Status.Failure;

        _aiMove = Self.Value.GetComponent<AIMovement>();
        if (_aiMove == null) return Status.Failure;

        _aiMove.SetSpeed(3.5f);
        _timer = 0f;
        
        _isWaiting = false;
        SetNewRandomDestination(); 

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_aiMove == null) return Status.Failure;
        
        _timer += Time.deltaTime;
        if (_timer >= CooldownTime.Value)
        {
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
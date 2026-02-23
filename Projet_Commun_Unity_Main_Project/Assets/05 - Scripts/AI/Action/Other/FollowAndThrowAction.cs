using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FollowAndThrowAction", story: "[Self] chases, catches [Targetplayer] waits [HoldTime] and throws with [ThrowForce]", category: "Action", id: "c383ce32771474b6b8d11dfd24c7834f")]
public partial class FollowAndThrowAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetPlayer;
    
    [SerializeReference] public BlackboardVariable<float> CatchDistance = new BlackboardVariable<float>(1.5f);
    [SerializeReference] public BlackboardVariable<float> HoldTime = new BlackboardVariable<float>(3.5f);
    [SerializeReference] public BlackboardVariable<float> ThrowForce = new BlackboardVariable<float>(15f);
    
    [SerializeReference] public BlackboardVariable<string> FloorTag = new BlackboardVariable<string>("LostLuggageFloor");

    private AIMovement _aiMove;
    private Controller _aiController;
    private IGrabbable _playerGrabbable;
    
    private float _timer;
    private float _repathTimer; 
    private enum Phase { Chasing, Holding }
    private Phase _phase;

    protected override Status OnStart()
    {
        if (Self.Value == null || TargetPlayer.Value == null) return Status.Failure;

        _aiMove = Self.Value.GetComponent<AIMovement>();
        _aiController = Self.Value.GetComponent<Controller>();
        _playerGrabbable = TargetPlayer.Value.GetComponent<IGrabbable>();
        
        if (_aiMove == null || _aiController == null || _playerGrabbable == null) return Status.Failure;

        _phase = Phase.Chasing;
        _timer = 0f;
        _repathTimer = 0f;

        _aiMove.SetSpeed(6f);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (TargetPlayer.Value == null) return Status.Failure;

        if (_phase == Phase.Chasing)
        {
            _repathTimer += Time.deltaTime;
            if (_repathTimer >= 0.15f)
            {
                _aiMove.SetDestination(TargetPlayer.Value.transform.position);
                _repathTimer = 0f;
            }

            float dist = Vector3.Distance(Self.Value.transform.position, TargetPlayer.Value.transform.position);
            
            if (dist <= CatchDistance.Value)
            {
                CatchPlayer();
            }
        }
        else if (_phase == Phase.Holding)
        {
            _timer += Time.deltaTime;
            
            if (_aiMove.HasReachedDestination() && _timer < HoldTime.Value - 0.5f)
            {
                if (GetRandomPointOnNavMesh(Self.Value.transform.position, 10f, out Vector3 result))
                {
                    _aiMove.SetDestination(result);
                }
            }

            if (_timer >= HoldTime.Value)
            {
                ThrowPlayer();
                return Status.Success; 
            }
        }

        return Status.Running;
    }

    private void CatchPlayer()
    {
        _phase = Phase.Holding;
        _playerGrabbable.Grabbed(_aiController);

        _aiMove.SetSpeed(3.5f);

        if (GetRandomPointOnNavMesh(Self.Value.transform.position, 10f, out Vector3 result))
        {
            _aiMove.SetDestination(result);
        }
    }

    private void ThrowPlayer()
    {
        Vector3 throwDirection = Self.Value.transform.forward + Vector3.up * 0.5f;
        _playerGrabbable.Dropped(throwDirection.normalized * ThrowForce.Value, _aiController);
        _aiMove.Stop();
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
using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SmartQueueMovement", story: "[self] advances in [locations] queue smoothly", category: "Action", id: "SmartQueueMovement")]
public partial class SmartQueueMovementAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Location; 
    [SerializeReference] public BlackboardVariable<List<GameObject>> Locations;
    [SerializeReference] public BlackboardVariable<bool> IsLast;
    [SerializeReference] public BlackboardVariable<float> StoppingDistance = new BlackboardVariable<float>(0.5f);
    
    [SerializeReference] public BlackboardVariable<float> CheckInterval = new BlackboardVariable<float>(0.2f);

    private AIMovement _aiMove;
    private float _timer;
    private bool _hasStartedMoving = false;

    protected override Status OnStart()
    {
        if (Self.Value == null || Locations.Value == null || Locations.Value.Count == 0)
            return Status.Failure;

        _aiMove = Self.Value.GetComponent<AIMovement>();
        if (_aiMove == null) return Status.Failure;
        
        _hasStartedMoving = false;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_aiMove == null || Locations.Value == null) return Status.Failure;
        
        if (!_hasStartedMoving)
        {
            _hasStartedMoving = true;
            
            CheckForBetterSpot();
            if (Location.Value != null)
            {
                _aiMove.SetDestination(Location.Value.transform.position);
            }
        }
        
        _timer += Time.deltaTime;
        if (_timer >= CheckInterval.Value)
        {
            _timer = 0f;
            if (CheckForBetterSpot())
            {
                if (Location.Value != null)
                {
                    _aiMove.SetDestination(Location.Value.transform.position);
                }
            }
            else if (Location.Value != null)
            {
                float distToTarget = Vector3.Distance(Self.Value.transform.position, Location.Value.transform.position);
                if (distToTarget > StoppingDistance.Value * 1.5f)
                {
                    _aiMove.SetDestination(Location.Value.transform.position);
                }
            }
        }
        
        if (Location.Value == null) return Status.Failure;
        
        float dist = Vector3.Distance(Self.Value.transform.position, _aiMove.GetDestination());
        bool hasReached = dist <= StoppingDistance.Value;

        if (IsLast.Value && hasReached)
        {
            return Status.Success; 
        }

        return Status.Running;
    }

    private bool CheckForBetterSpot()
    {
        var list = Locations.Value;
        GameObject currentObj = Location.Value;
        
        int currentIndex = -1;
        if (currentObj != null)
        {
            currentIndex = list.IndexOf(currentObj);
        }

        if (currentIndex >= list.Count - 1)
        {
            if (!IsLast.Value) IsLast.Value = true;
            return false;
        }

        IsLast.Value = false;
        int bestTargetIndex = currentIndex;

        for (int i = currentIndex + 1; i < list.Count; i++)
        {
            GameObject checkObj = list[i];
            if (checkObj == null) break;

            LocationPoint checkPoint = checkObj.GetComponent<LocationPoint>();
            
            if (checkPoint != null && checkPoint.available)
            {
                bestTargetIndex = i;
            }
            else
            {
                break; 
            }
        }

        if (bestTargetIndex > currentIndex)
        {
            if (currentObj != null)
            {
                LocationPoint oldPoint = currentObj.GetComponent<LocationPoint>();
                if (oldPoint != null) oldPoint.available = true;
            }
            
            GameObject newTargetObj = list[bestTargetIndex];
            LocationPoint newTargetPoint = newTargetObj.GetComponent<LocationPoint>();
            if (newTargetPoint != null) newTargetPoint.available = false;
            
            Location.Value = newTargetObj; 

            if (bestTargetIndex == list.Count - 1) IsLast.Value = true;
            else IsLast.Value = false;

            return true;
        }

        return false;
    }
}
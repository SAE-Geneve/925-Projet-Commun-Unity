using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckNextLocation", story: "[self] check available path in [locations]", category: "Action",
    id: "8cad99c8ce20d75b2bb61b273546f873")]
public partial class CheckNextLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Location;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Locations;
    [SerializeReference] public BlackboardVariable<bool> IsLast;

    [SerializeReference] public BlackboardVariable<float> StoppingDistance = new BlackboardVariable<float>(1.0f);

    protected override Status OnStart()
    {
        if (Self.Value == null || Locations.Value == null || Locations.Value.Count == 0)
        {
            return Status.Failure;
        }
        
        int currentIndex = -1;
        if (Location.Value != null)
        {
            currentIndex = Locations.Value.IndexOf(Location.Value);
        }
        
        if (currentIndex >= Locations.Value.Count - 1)
        {
            IsLast.Value = true;
            float dist = Vector3.Distance(Self.Value.transform.position, Location.Value.transform.position);
            return dist <= StoppingDistance.Value ? Status.Failure : Status.Success;
        }

        IsLast.Value = false;
        
        int bestTargetIndex = currentIndex;

        for (int i = currentIndex + 1; i < Locations.Value.Count; i++)
        {
            GameObject checkObj = Locations.Value[i];
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
            if (Location.Value != null)
            {
                LocationPoint currentPoint = Location.Value.GetComponent<LocationPoint>();
                if (currentPoint != null) currentPoint.available = true;
            }
            
            GameObject targetObj = Locations.Value[bestTargetIndex];
            LocationPoint targetPoint = targetObj.GetComponent<LocationPoint>();

            if (targetPoint != null) targetPoint.available = false;

            Location.Value = targetObj;
            
            if (bestTargetIndex == Locations.Value.Count - 1) IsLast.Value = true;

            return Status.Success;
        }
        if (Location.Value == null)
        {
            return Status.Failure;
        }
        
        float distance = Vector3.Distance(Self.Value.transform.position, Location.Value.transform.position);
        if (distance <= StoppingDistance.Value)
        {
            return Status.Failure;
        }

        return Status.Success;
    }
}
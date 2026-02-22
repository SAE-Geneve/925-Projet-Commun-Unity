using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PreAllocLocation", story: "pre alloc a [location] in [Locations]", category: "Action", id: "38b5c69bc0ab25ae43d8e59200a5fbee")]
public partial class PreAllocLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Location;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Locations;
    [SerializeReference] public BlackboardVariable<float> CheckInterval = new BlackboardVariable<float>(0.5f);

    private float _timer;

    protected override Status OnStart()
    {
        if (Locations.Value == null || Locations.Value.Count == 0)
            return Status.Failure;

        // On initialise le timer au maximum pour forcer un check immédiat à la première frame
        _timer = CheckInterval.Value; 
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Locations.Value == null || Locations.Value.Count == 0)
            return Status.Failure;

        _timer += Time.deltaTime;
        
        // Si le temps n'est pas écoulé, on met l'action en pause sans rien vérifier
        if (_timer < CheckInterval.Value)
        {
            return Status.Running;
        }

        _timer = 0f;
        int bestTargetIndex = -1;

        for (int i = 0; i < Locations.Value.Count; i++)
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

        if (bestTargetIndex != -1)
        {
            GameObject targetObj = Locations.Value[bestTargetIndex];
            LocationPoint targetPoint = targetObj.GetComponent<LocationPoint>();
            if (targetPoint != null) targetPoint.available = false;

            Location.Value = targetObj;
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}
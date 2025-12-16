using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LocationRotation", story: "Set [agent] rotation to [Location] rotation", category: "Action", id: "1807701abbdd2a30dde72865db3707da")]
public partial class LocationRotationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Location;

    protected override Status OnStart()
    {
        GameObject agentObj = Agent.Value;
        GameObject locationObj = Location.Value;
        
        if(agentObj == null || locationObj == null) return Status.Failure;

        if (locationObj.TryGetComponent(out LocationPoint locationPoint))
        {
            agentObj.transform.rotation =
                Quaternion.LookRotation(locationPoint.Forward, Vector3.up);
                
            return Status.Success;
        }
        
        return Status.Failure;
    }
}


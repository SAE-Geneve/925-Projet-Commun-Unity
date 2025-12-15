using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FreeLocation", story: "free the [location]", category: "Action",
    id: "8b91726c74be77117735347152cbab2a")]
public partial class FreeLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Location;

    protected override Status OnStart()
    {
        var locationPoint = Location.Value.GetComponent<LocationPoint>();
        if (locationPoint != null)
        {
            locationPoint.available = true;
        }

        return Status.Success;
    }
}


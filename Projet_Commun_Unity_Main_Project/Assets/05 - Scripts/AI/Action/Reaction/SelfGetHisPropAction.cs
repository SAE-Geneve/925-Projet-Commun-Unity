using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfGetHisProp", story: "[Self] receives a Prop in his [GrabZone] matching his [PreferredType] and frees its [Location]", category: "Action", id: "d1a7b6f2c9e347eaa1b2d3c4e5f67890")]
public partial class SelfGetHisPropAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> GrabZone;
    [SerializeReference] public BlackboardVariable<PropTypeBlackBoard> PreferredType;
    [SerializeReference] public BlackboardVariable<GameObject> Location;
    protected override Status OnUpdate()
    {
        if (Self?.Value == null)
            return Status.Failure;

        Collider selfCol = GrabZone.Value.GetComponent<Collider>();
        if (selfCol == null)
            return Status.Failure;

        Collider[] hits = Physics.OverlapBox(
            selfCol.bounds.center,
            selfCol.bounds.extents,
            Self.Value.transform.rotation
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value)
                continue;

            var prop = hit.GetComponent<Prop>();
            if (prop == null || prop.IsGrabbed)
                continue;
            
            if (hit.gameObject.name.Contains("Clone"))
            {
              //  Debug.Log($"Prop détecté : {prop.name}, Type = {prop.Type}, PreferredType = {PreferredType.Value}");
            }
            
            if (IsMatchingPreferred(prop.Type, PreferredType.Value))
            {
                if (hit.gameObject.name.Contains("Clone"))
                {
                    Debug.Log($"{Self.Value.name} a récupéré la bonne valise ({prop.Type}) -> SUCCESS");
                }


                if (Location?.Value != null)
                {
                    var locationPoint = Location.Value.GetComponent<LocationPoint>();
                    if (locationPoint != null)
                    {
                        locationPoint.available = true;
                        if (Location.Value.name.Contains("Clone"))
                            Debug.Log($"Location {Location.Value.name} libérée");
                    }
                }

                return Status.Success;
            }
        }

        return Status.Running;
    }

    private bool IsMatchingPreferred(PropType propType, PropTypeBlackBoard preferred)
    {
        return preferred switch
        {
            PropTypeBlackBoard.RedLuggage   => propType == PropType.RedLuggage,
            PropTypeBlackBoard.BlueLuggage  => propType == PropType.BlueLuggage,
            PropTypeBlackBoard.GreenLuggage => propType == PropType.GreenLuggage,
            PropTypeBlackBoard.YellowLuggage=> propType == PropType.YellowLuggage,
            _ => false
        };
    }
}

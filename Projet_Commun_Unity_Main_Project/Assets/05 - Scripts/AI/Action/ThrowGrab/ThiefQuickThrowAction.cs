using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Thief Quick Throw", story: "[Npc] throws [Bag] with [Force]", category: "Action", id: "thief_quick_throw")]
public partial class ThiefQuickThrowAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Npc;
    [SerializeReference] public BlackboardVariable<GameObject> Bag;
    [SerializeReference] public BlackboardVariable<float> Force;

    protected override Status OnStart()
    {
        if (Npc?.Value == null || Bag?.Value == null)
            return Status.Failure;
        
        Controller controller = Npc.Value.GetComponent<Controller>();
        if (Bag.Value.TryGetComponent(out IGrabbable grabbable))
        {
            Vector3 launchDirection = (Npc.Value.transform.forward + Vector3.up * 0.5f).normalized;
            Vector3 launchForce = launchDirection * Force.Value;
            grabbable.Dropped(launchForce, controller);
            
            return Status.Success;
        }

        return Status.Failure;
    }
}
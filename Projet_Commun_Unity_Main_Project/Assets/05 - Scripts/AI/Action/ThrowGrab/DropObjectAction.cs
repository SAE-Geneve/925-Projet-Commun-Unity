using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Drop Object", story: "[Npc] drops the object they are holding", category: "Action", id: "ai_drop_object_action")]
public partial class DropObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<AIController> Npc;

    protected override Status OnStart()
    {
        if (Npc == null || Npc.Value == null) return Status.Failure;
        AIController controller = Npc.Value;

        if (controller != null)
        {
            controller.Drop();
            return Status.Success;
        }

        return Status.Failure;
    }
}
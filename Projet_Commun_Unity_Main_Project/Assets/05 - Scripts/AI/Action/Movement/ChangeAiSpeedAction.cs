using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangeAISpeed", story: "change the [ai] speed", category: "Action", id: "870285ccdc51c82a602e0f5014f18f10")]
public partial class ChangeAiSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<AIMovement> Ai;
    [SerializeReference] public BlackboardVariable<float> speed;

    protected override Status OnStart()
    {
        Ai.Value.speed = speed;
        return Status.Success;
    }
}


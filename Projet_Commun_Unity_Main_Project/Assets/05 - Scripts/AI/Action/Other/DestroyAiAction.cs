using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Destroy AI", story: "Destroy [AI]", category: "Action", id: "bb80c3913aa3fa1d0312c768a2efe988")]
public partial class DestroyAiAction : Action
{
    [SerializeReference] public BlackboardVariable<AIController> AI;

    protected override Status OnStart()
    {
        if (AI.Value == null)
        {
            LogFailure("No valid AI to destroy provided.");
            return Status.Failure;
        }
        
        AI.Value.DestroyAI();
        return Status.Success;
    }
}


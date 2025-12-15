using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Fail Task", story: "Failing [Task]", category: "Action", id: "1c1e40847f4428740d7627bb07eabddb")]
public partial class FailTaskAction : Action
{
    [SerializeReference] public BlackboardVariable<GameTask> Task;

    protected override Status OnStart()
    {
        if(Task.Value == null) return Status.Failure;
        
        Task.Value.Failed();
        return Status.Success;
    }
}


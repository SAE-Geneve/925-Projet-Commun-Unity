using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Task", story: "[Agent] wait his task", category: "Action", id: "892481ea1819dffbd06ae2f4723f523a")]
public partial class TaskAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    private GameTask _task;
    
    private System.Action _onSucceedHandler;
    private System.Action _onFailedHandler;
    
    private bool _succeed;
    private bool _failed;

    protected override Status OnStart()
    {
        _task = Agent.Value.GetComponent<GameTask>();
        
        if(!_task) return Status.Failure;
        
        _onSucceedHandler = () => _succeed = true;
        _onFailedHandler = () => _failed = true;

        _task.OnSucceedAction += _onSucceedHandler;
        _task.OnFailedAction += _onFailedHandler;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        
        if(_succeed ) return Status.Success;
        
        return _failed ? Status.Failure : Status.Running;
    }

    protected override void OnEnd()
    {
        _succeed = false;
        _failed = false;
        
        if(!_task) return;
        
        _task.OnSucceedAction -= _onSucceedHandler;
        _task.OnFailedAction -= _onFailedHandler;
    }
}


using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlaneAiIsAngry", story: "change the [ground] color to angry", category: "Action", id: "08bb69ef3b9ae082f7dfa1afc2ce427e")]
public partial class PlaneAiIsAngryAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Ground;
    protected override Status OnStart()
    {
        var groundRenderer = Ground.Value.GetComponent<Renderer>();
        if (groundRenderer != null)
            groundRenderer.material.color = Color.black;
        
        return Status.Running;
    }
}


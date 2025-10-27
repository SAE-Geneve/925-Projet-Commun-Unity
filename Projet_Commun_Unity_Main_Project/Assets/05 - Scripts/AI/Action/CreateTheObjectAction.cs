using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Create the object", story: "Spawn [thing] from [Theses] at [Pos] using [ColorToSpawn] and change the [Ground]", category: "Action", id: "8760ff406272473d928adc701fb9c664")]
public partial class CreateTheObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Theses;
    [SerializeReference] public BlackboardVariable<Transform> Pos;
    [SerializeReference] public BlackboardVariable<Color> ColorToSpawn;
    [SerializeReference] public BlackboardVariable<GameObject> Ground;

    protected override Status OnStart()
    {
        if (Theses?.Value == null || Theses.Value.Count == 0 || Pos?.Value == null)
            return Status.Failure;
        
        var prefab = Theses.Value[UnityEngine.Random.Range(0, Theses.Value.Count)];
        var spawned = GameObject.Instantiate(prefab, Pos.Value.position, Quaternion.identity);
        
        Color randomColor;
        int choice = UnityEngine.Random.Range(0, 3);
        switch (choice)
        {
            case 0: randomColor = Color.red; break;
            case 1: randomColor = Color.green; break;
            default: randomColor = Color.blue; break;
        }

        var renderer = spawned.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(renderer.material);
            renderer.material.color = randomColor;
            
            if (Ground?.Value != null)
            {
                var groundRenderer = Ground.Value.GetComponent<Renderer>();
                if (groundRenderer != null)
                    groundRenderer.material.color = randomColor;
            }
        }
        ColorToSpawn.Value = randomColor;

        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
}
using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnLuggage", story: "Spawn the [npc] luggage from [prefabs]", category: "Action", id: "ead0ed41a609c25af23cfb5ca75c1cdd")]
public partial class SpawnLuggageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Npc;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Prefabs;
    protected override Status OnStart()
    {
        if (Npc?.Value == null || Prefabs?.Value == null || Prefabs.Value.Count == 0)
            return Status.Failure;

        Transform npcTransform = Npc.Value.transform;
        
        GameObject prefab = Prefabs.Value[UnityEngine.Random.Range(0, Prefabs.Value.Count)];
        
        Vector3 spawnPos = npcTransform.position + npcTransform.forward * 1.7f + Vector3.up * 0.5f;
        
        GameObject.Instantiate(prefab, spawnPos, npcTransform.rotation);
        

        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
}


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
        if (Npc == null || Npc.Value == null)
        {
            Debug.LogWarning("SpawnLuggageAction: Npc variable est NULL ou sa valeur est NULL. Échec.", Npc?.Value);
            return Status.Failure;
        }
        
        if (Prefabs == null || Prefabs.Value == null)
        {
            Debug.LogWarning($"SpawnLuggageAction sur {Npc.Value.name}: Prefabs variable est NULL. Échec.");
            return Status.Failure;
        }
        
        if (Prefabs.Value.Count == 0)
        {
            Debug.LogWarning($"SpawnLuggageAction sur {Npc.Value.name}: Liste de Prefabs est vide. Échec.");
            return Status.Failure;
        }

        
        Transform npcTransform = Npc.Value.transform;
        
        int prefabIndex = UnityEngine.Random.Range(0, Prefabs.Value.Count);
        GameObject prefab = Prefabs.Value[prefabIndex];

        if (prefab == null)
        {
            return Status.Failure;
        }

        Vector3 spawnPos = npcTransform.position + npcTransform.forward * 1.7f + Vector3.up * 0.5f;
        
        GameObject newLuggage = GameObject.Instantiate(prefab, spawnPos, npcTransform.rotation);
        
        if (newLuggage == null)
        {
            return Status.Failure;
        }
        
        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
}
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Spawn Object", story: "Spawn the [Prefab] near [Npc]", category: "Action", id: "spawn_single_object_action")]
public partial class SpawnObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Prefab;
    [SerializeReference] public BlackboardVariable<GameObject> Npc;

    protected override Status OnStart()
    {
        if (Npc == null || Npc.Value == null)
        {
            Debug.LogWarning("SpawnObjectAction: Npc variable est NULL ou sa valeur est NULL. Échec.");
            return Status.Failure;
        }
        if (Prefab == null || Prefab.Value == null)
        {
            Debug.LogWarning($"SpawnObjectAction sur {Npc.Value.name}: Prefab variable est NULL. Échec.");
            return Status.Failure;
        }

        Transform npcTransform = Npc.Value.transform;
        Vector3 spawnPos = npcTransform.position + npcTransform.forward * 1.5f + Vector3.up * 0.5f;
        GameObject newObject = GameObject.Instantiate(Prefab.Value, spawnPos, npcTransform.rotation);

        if (newObject == null)
        {
            return Status.Failure;
        }

        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
}
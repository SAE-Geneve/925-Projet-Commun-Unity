using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "SpawnWithLuggage",
    story: "Give the [npc] his [luggage] from [prefabs]",
    category: "Action",
    id: "01b1cee4cb8250c1080bfecf909ba574")]
public partial class SpawnWithLuggageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Npc;
    [SerializeReference] public BlackboardVariable<GameObject> Luggage;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Prefabs;

    private Controller controller;

    protected override Status OnStart()
    {
        if (Npc?.Value == null || Prefabs?.Value == null || Prefabs.Value.Count == 0)
            return Status.Failure;

        controller = Npc.Value.GetComponent<Controller>();
        if (controller == null)
            return Status.Failure;

        // random prefab
        GameObject prefab = Prefabs.Value[UnityEngine.Random.Range(0, Prefabs.Value.Count)];

        // instantiation directement au niveau du _catchPoint
        Transform attachPoint = controller.CatchPoint;
        if (attachPoint == null)
            return Status.Failure;

        GameObject lug = GameObject.Instantiate(prefab, attachPoint.position, attachPoint.rotation);

        // stockage dans Blackboard
        Luggage.Value = lug;

        // attachement automatique
        if (lug.TryGetComponent(out IGrabbable grabbable))
        {
            grabbable.Grabbed(controller);
        }

        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
}
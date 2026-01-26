using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomBool", story: "Set [Bool] with [Chance] % to be true", category: "Action", id: "816eae8dfcfddb805c41a8ebf205a23f")]
public partial class RandomBoolAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool;
    [SerializeReference] public BlackboardVariable<float> Chance = new BlackboardVariable<float>(0.5f); 

    protected override Status OnStart()
    {
        Bool.Value = UnityEngine.Random.value < Chance.Value;

        return Status.Success;
    }
}


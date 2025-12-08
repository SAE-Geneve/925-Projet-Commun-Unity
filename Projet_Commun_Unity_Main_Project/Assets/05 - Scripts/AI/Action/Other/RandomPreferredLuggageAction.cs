using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "RandomPreferredLuggage",
    story: "Selects a random luggage type and colors [Indicator] accordingly, storing result in [PreferredType]",
    category: "Action",
    id: "3c27abc38ae3c5f1e44e5bc5b870933f")]
public partial class RandomPreferredLuggageAction : Action
{
    [SerializeReference] public BlackboardVariable<PropTypeBlackBoard> PreferredType;
    [SerializeReference] public BlackboardVariable<GameObject> Indicator;

    private static readonly PropTypeBlackBoard[] PossibleColors = new[]
    {
        PropTypeBlackBoard.RedLuggage,
        PropTypeBlackBoard.BlueLuggage,
        PropTypeBlackBoard.GreenLuggage,
        PropTypeBlackBoard.YellowLuggage
    };

    protected override Status OnStart()
    {
        if (PreferredType == null)
        {
            Debug.LogWarning("PreferredType variable missing !");
            return Status.Failure;
        }

        if (Indicator?.Value == null)
        {
            Debug.LogWarning("Indicator (cube) reference missing !");
            return Status.Failure;
        }
        
        int index = UnityEngine.Random.Range(0, PossibleColors.Length);
        PreferredType.Value = PossibleColors[index];
        
        ApplyIndicatorColor(Indicator.Value, PreferredType.Value);
        
        Debug.Log($"[RandomPreferred] Assigned PreferredType = {PreferredType.Value}");
        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;

    private void ApplyIndicatorColor(GameObject cube, PropTypeBlackBoard type)
    {
        var renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(renderer.material);

            renderer.material.color = type switch
            {
                PropTypeBlackBoard.RedLuggage    => Color.red,
                PropTypeBlackBoard.BlueLuggage   => Color.blue,
                PropTypeBlackBoard.GreenLuggage  => Color.green,
                PropTypeBlackBoard.YellowLuggage => Color.yellow,
                _ => Color.white
            };

            Debug.Log($"[CubeColor] {cube.name} color set to {renderer.material.color} for type {type}");
        }
        else
        {
            Debug.LogWarning($"Cube {cube.name} n'a pas de Renderer !");
        }
    }

}

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfGetHis", story: "[Self] receives an object matching his [PreferredColor] from [AllLuggages]", category: "Action", id: "cc55e02ef5337cc157b5a416aa467400")]
public partial class SelfGetHisAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Color> PreferredColor;
    protected override Status OnUpdate()
    {
        if (Self?.Value == null)
            return Status.Failure;

        Collider selfCol = Self.Value.GetComponent<Collider>();
        if (selfCol == null)
        {
            return Status.Failure;
        }
        
        Collider[] hits = Physics.OverlapBox(
            selfCol.bounds.center,
            selfCol.bounds.extents,
            Self.Value.transform.rotation
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject == Self.Value)
                continue;

            var renderer = hit.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            Debug.Log($"⚡ {Self.Value.name} touche {hit.name} avec couleur {renderer.material.color}");

            // Vérifie si la couleur correspond
            if (SameColor(renderer.material.color, PreferredColor.Value))
            {
                Debug.Log($"{Self.Value.name} a reçu la bonne valise ✅ ({hit.name})");
                GameObject.Destroy(hit.gameObject);
                return Status.Success;
            }
        }

        return Status.Running;
    }

    private bool SameColor(Color a, Color b, float tolerance = 0.05f)
    {
        return Vector3.Distance(new Vector3(a.r, a.g, a.b), new Vector3(b.r, b.g, b.b)) < tolerance;
    }
}


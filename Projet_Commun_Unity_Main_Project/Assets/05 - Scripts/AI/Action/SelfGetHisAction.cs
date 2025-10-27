using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelfGetHis", story: "[Self] receives an object matching his [PreferredColor] and change [ground] color", category: "Action", id: "cc55e02ef5337cc157b5a416aa467400")]
public partial class SelfGetHisAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Color> PreferredColor;
    [SerializeReference] public BlackboardVariable<GameObject> Ground;
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

            var prop = hit.GetComponent<Prop>();
            if (prop == null || prop.IsGrabbed)
                continue; // Ignore si ce n’est pas un Prop ou s’il est tenu

            var renderer = hit.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            if (!SameColor(renderer.material.color, PreferredColor.Value))
                continue;

            Debug.Log($"{Self.Value.name} a reçu la bonne valise ✅ ({hit.name})");
            
            var groundRenderer = Ground.Value.GetComponent<Renderer>();
            if (groundRenderer != null)
                groundRenderer.material.color = Color.white;
            
            UnityEngine.Object.Destroy(hit.gameObject);
            return Status.Success;
        }


        return Status.Running;
    }

    private bool SameColor(Color a, Color b, float tolerance = 0.05f)
    {
        return Vector3.Distance(new Vector3(a.r, a.g, a.b), new Vector3(b.r, b.g, b.b)) < tolerance;
    }
}


using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "PutTheLuggage",
    story: "[npc] throw the [luggage] on the [thing]",
    category: "Action",
    id: "9f995efbff4ce4af14d6906e31fd5ccc")]
public partial class PutTheLuggageAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Npc;
    [SerializeReference] public BlackboardVariable<GameObject> Luggage;
    [SerializeReference] public BlackboardVariable<Transform> Thing;
    
    [SerializeReference] public BlackboardVariable<float> turnDuration;
    [SerializeReference] public BlackboardVariable<float> throwForce;


    private Controller controller;
    private Transform npcTransform;
    private IGrabbable grabbable;

    private enum Phase { Turning, Throwing, Done }
    private Phase phase;

    private Quaternion startRot;
    private Quaternion targetRot;
    private float turnTime = 0f;
    
    protected override Status OnStart()
    {
        if (Npc?.Value == null || Luggage?.Value == null || Thing?.Value == null)
            return Status.Failure;

        controller = Npc.Value.GetComponent<Controller>();
        npcTransform = Npc.Value.transform;

        if (!Luggage.Value.TryGetComponent(out grabbable))
            return Status.Failure;

        // direction vers le tapis (ignorer la hauteur)
        Vector3 dir = Thing.Value.position - npcTransform.position;
        dir.y = 0f; // on bloque la rotation horizontale
        if (dir.sqrMagnitude < 0.01f) dir = npcTransform.forward;

        startRot = npcTransform.rotation;
        targetRot = Quaternion.LookRotation(dir);
        turnTime = 0f;
        phase = Phase.Turning;

        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        switch (phase)
        {
            case Phase.Turning:
                turnTime += Time.deltaTime;
                float t = Mathf.Clamp01(turnTime / turnDuration.Value);
                npcTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);

                if (t >= 1f)
                {
                    phase = Phase.Throwing;
                    return Status.Running;
                }
                break;

            case Phase.Throwing:
                Vector3 force = npcTransform.forward * throwForce.Value;

                grabbable.Dropped(force, controller);

                phase = Phase.Done;
                return Status.Success;
        }

        return Status.Running;
    }
}

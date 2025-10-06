using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "goToPlayer", story: "[self] go to [player]", category: "Action", id: "7eadbb9e76c3dd54f092186fa2775185")]
public partial class GoToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    public float speed = 3f; // vitesse de déplacement
    public float stopDistance = 1.5f; // distance pour considérer qu'on est arrivé

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Player?.Value == null)
            return Status.Failure;

        Vector3 selfPos = Self.Value.transform.position;
        Vector3 playerPos = Player.Value.transform.position;

        // Direction vers le player
        Vector3 direction = (playerPos - selfPos).normalized;

        // Déplacement simple
        Self.Value.transform.position += direction * speed * Time.deltaTime;

        // Vérifie si Self est assez proche du player
        float distance = Vector3.Distance(selfPos, playerPos);
        if (distance <= stopDistance)
        {
            return Status.Success; // arrivé
        }

        return Status.Running; // continue à se déplacer
    }

    protected override void OnEnd()
    {
        // Pas d'action spécifique à la fin
    }
}
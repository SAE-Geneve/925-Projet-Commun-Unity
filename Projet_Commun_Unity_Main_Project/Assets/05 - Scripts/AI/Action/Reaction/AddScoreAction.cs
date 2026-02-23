    using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AddScore", story: "Add [score] to the [manager] if [enemy] is [good]", category: "Action", id: "b5e0770f2b355be97454a9b8fe077040")]
public partial class AddScoreAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Score;
    [SerializeReference] public BlackboardVariable<InteractableTask> Manager;
    [SerializeReference] public BlackboardVariable<bool> Enemy;
    [SerializeReference] public BlackboardVariable<bool> Good;
    protected override Status OnStart()
    {
        Debug.LogWarning(Enemy.Value == Good.Value);
        if (Enemy.Value == Good.Value)
        {
            GameManager.Instance.Scores.AddMissionScore(Score, Manager.Value.PlayerController.Id);
        }
        return Status.Success;
       
    }
}


using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check If Caught", story: "Check if [Ragdoll] was [caught] by a player and give [Reward] points", category: "Action", id: "check_if_caught_action")]
public partial class CheckIfCaughtAction : Action
{
    [SerializeReference] public BlackboardVariable<AIRagdoll> Ragdoll;
    [SerializeReference] public BlackboardVariable<bool> Caught;
    [SerializeReference] public BlackboardVariable<int> Reward;
    
    [SerializeReference] public BlackboardVariable<bool> IsLostBool;

    protected override Status OnStart()
    {
        if (Ragdoll == null || Ragdoll.Value == null) return Status.Failure;
        return CheckStatus();
    }

    protected override Status OnUpdate()
    {
        return CheckStatus();
    }

    private Status CheckStatus()
    {
        if (Ragdoll.Value == null) return Status.Failure;
        PlayerController catcher = Ragdoll.Value.Catcher;

        if (catcher != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.Scores != null)
            {
                if (GameManager.Instance.Context == GameContext.Hub)
                    GameManager.Instance.Scores.AddPlayerScore(Reward.Value, catcher.Id);
                else
                    GameManager.Instance.Scores.AddPlayerScore(Reward.Value, catcher.Id);
                    
                Debug.Log($"[Score] Le joueur {catcher.Id} gagne {Reward.Value} points !");
            }
            Ragdoll.Value.ClearCatcher();
            
            if (Caught != null) Caught.Value = true;
            
            return Status.Success;
        }
        
        if (Ragdoll.Value.IsRagdollState)
        {
            if (IsLostBool != null) IsLostBool.Value = true;
        }

        if (Caught != null) Caught.Value = false;
        return Status.Running; 
    }
}
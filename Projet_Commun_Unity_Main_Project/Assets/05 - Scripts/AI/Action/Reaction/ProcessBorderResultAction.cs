using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ProcessBorderResult", story: "Feedback [ui] from [IsAccepted] and [IsEvil] (Border)", category: "Action", id: "f48f8ac4d3f59ea42e3466afeeddec0f")]
public partial class ProcessBorderResultAction : Action
{
    [SerializeReference] public BlackboardVariable<BorderNPCUI> Ui;
    [SerializeReference] public BlackboardVariable<bool> IsAccepted;
    [SerializeReference] public BlackboardVariable<bool> IsEvil;

    protected override Status OnStart()
    {
        bool accepted = IsAccepted.Value;
        bool evil = IsEvil.Value;
        bool success = false;
        
        if (accepted)
        {
            if (evil)
            {
                ScoreSystem.IncreaseScore(3);
            }
            else
            {
                success = true;
                ScoreSystem.IncreaseScore(1);
            }
        }
        else
        {
            if (evil)
            {
                success = true;
                ScoreSystem.IncreaseScore(1);
            }
            else
            {
                ScoreSystem.IncreaseScore(3);
            }
        }
        
        Ui.Value.PlayFeedback(success, evil, accepted);
        
        return Status.Success;
    }
}
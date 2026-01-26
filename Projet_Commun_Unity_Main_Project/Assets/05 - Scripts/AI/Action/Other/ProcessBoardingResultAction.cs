using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Process Boarding Result", story: "Feedback [ui] from [IsAccepted] and [IsEvil]", category: "Action", id: "ProcessResult")]
public partial class ProcessBoardingResultAction : Action
{
    [SerializeReference] public BlackboardVariable<BoardingNPCUI> Ui;
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
                ScoreSystem.IncreaseScore(2);
            }
            else
            {
                ScoreSystem.IncreaseScore(4);
            }
        }
        
        Ui.Value.PlayFeedback(success, evil, accepted);

        return Status.Success;
    }
}

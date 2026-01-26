using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Show Passport UI", story: "Display [ui] passport based on [IsEvil] and show", category: "Action", id: "ShowPassport")]
public partial class ShowPassportUiAction : Action
{
    [SerializeReference] public BlackboardVariable<BoardingNPCUI> Ui;
    [SerializeReference] public BlackboardVariable<bool> IsEvil;
    protected override Status OnStart()
    {
        // On trouve le script UI sur l'agent ou ses enfants
       
        
        if (Ui != null)
        {
            Ui.Value.ShowPassport(IsEvil.Value);
            return Status.Success;
        }
        
        return Status.Failure;
    }
}
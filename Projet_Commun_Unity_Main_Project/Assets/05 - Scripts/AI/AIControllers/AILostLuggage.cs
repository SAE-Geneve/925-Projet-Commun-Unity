using UnityEngine;
using Unity.Behavior;

public class AILostLuggage : AIController
{
    protected override void Awake()
    {
        base.Awake();

        BehaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (BehaviorAgent == null) return;
        
        GameObject luggageObj = GameObject.Find("Lost luggage");
        if (luggageObj != null)
        {
            BehaviorAgent.SetVariableValue("LostLuggage", luggageObj);
        }
        else
        {
            Debug.LogWarning("'Lost luggage' introuvable dans la scène pour l'AI LostLuggage", this);
        }
    }
}
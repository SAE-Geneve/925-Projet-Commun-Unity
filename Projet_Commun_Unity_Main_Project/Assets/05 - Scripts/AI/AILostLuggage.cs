using UnityEngine;
using Unity.Behavior;

public class AILostLuggage : AIController
{
    protected override void Awake()
    {
        base.Awake();

        _behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (_behaviorAgent == null) return;

        // Cherche l'objet "Lost luggage" dans la scène
        GameObject luggageObj = GameObject.Find("Lost luggage");
        if (luggageObj != null)
        {
            _behaviorAgent.SetVariableValue("LostLuggage", luggageObj);
        }
        else
        {
            Debug.LogWarning("'Lost luggage' introuvable dans la scène pour l'AI LostLuggage", this);
        }
    }
}
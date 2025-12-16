using UnityEngine;
using Unity.Behavior;
using UnityEngine.Serialization;

public class AIController : Controller
{
    public BehaviorGraphAgent BehaviorAgent {get; protected set;}
    protected virtual void Awake()
    {
        BehaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (BehaviorAgent == null)
            Debug.LogError($"BehaviorGraphAgent manquant sur {gameObject.name}", this);
    }
}
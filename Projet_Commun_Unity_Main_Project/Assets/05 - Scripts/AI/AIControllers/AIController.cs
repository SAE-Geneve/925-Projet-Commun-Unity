using System;
using UnityEngine;
using Unity.Behavior;

public class AIController : Controller
{
    public event Action<AIController> OnDestroyed;
    
    public BehaviorGraphAgent BehaviorAgent {get; protected set;}
    public GameTask GameTask {get; private set;}
    protected virtual void Awake()
    {
        BehaviorAgent = GetComponent<BehaviorGraphAgent>();
        GameTask = GetComponent<GameTask>();
        if (BehaviorAgent == null)
            Debug.LogError($"BehaviorGraphAgent manquant sur {gameObject.name}", this);
    }

    public void DestroyAI()
    {
        if(IsBeingHeld) Dropped();
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
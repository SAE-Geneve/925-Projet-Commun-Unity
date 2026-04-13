using System;
using UnityEngine;
using Unity.Behavior;
using UnityEngine.UI;

public class AIController : Controller, IGrabbable
{
    [Header("Grab Settings")]
    [Tooltip("Décoche cette case pour empêcher les joueurs d'attraper cette IA (ex: Voleur)")]
    [SerializeField] private bool canBeGrabbed = true;

    public event Action<AIController> OnDestroyed;
    public RawImage cctvRawImage;
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
    
    public new void Grabbed(Controller controller)
    {
        if (!canBeGrabbed) return;
        base.Grabbed(controller);
    }
}
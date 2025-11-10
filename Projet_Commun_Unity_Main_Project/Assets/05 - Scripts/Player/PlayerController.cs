using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    [Header("Events")] 
    public UnityEvent OnEnterKart;
    public UnityEvent OnExitKart;
    
    
    public PlayerInput Input { get; private set;}
    public KartController KartController { get; set; }
    public KartMovement KartMovement { get; set; }
    
    public KartPhysic KartPhysic { get; set; }

    protected override void Start()
    {
        Input = GetComponent<PlayerInput>();
        
        PlayerManager playerManager = PlayerManager.Instance;
        if(playerManager) transform.parent = playerManager.transform;
        
        base.Start();
    }
    
    public void TryInteract()
    {
        if (InteractableGrabbed != null)
        {
            InteractableGrabbed.Interact(this);
            return;
        }
        
        TryAction<IInteractable>(interactable =>
        {
            interactable.Interact(this);
        });
    }

    public void TryEndInteract()
    {
        if (InteractableGrabbed != null)
            InteractableGrabbed.InteractEnd();
    }
}
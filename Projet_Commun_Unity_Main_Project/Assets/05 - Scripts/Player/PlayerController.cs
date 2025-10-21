using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public PlayerInput Input { get; private set;}

    protected override void Start()
    {
        Input = GetComponent<PlayerInput>();
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
}
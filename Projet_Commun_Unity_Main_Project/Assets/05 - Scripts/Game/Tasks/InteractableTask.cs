public class InteractableTask : GameTask, IInteractable
{
    public void Interact(PlayerController playerController)
    {
        if (Done) return;
        
        Succeed();
    }
}

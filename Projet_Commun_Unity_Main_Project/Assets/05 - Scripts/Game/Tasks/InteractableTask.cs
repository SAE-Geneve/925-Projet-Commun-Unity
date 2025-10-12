public class InteractableTask : GameTask, IInteractable
{
    public void Interact()
    {
        if (Done) return;
        
        Succeed();
    }
}

using UnityEngine;

public class InteractableTask : GameTask, IInteractable
{
    public void Interact(GameObject interactor)
    {
        if (Done) return;
        
        Succeed();
    }
}

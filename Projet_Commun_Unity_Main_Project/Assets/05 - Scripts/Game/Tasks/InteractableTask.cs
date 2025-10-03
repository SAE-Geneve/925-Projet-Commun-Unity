using UnityEngine;

public class InteractableTask : GameTask, IInteractable
{
    public void Interact()
    {
        Debug.LogWarning("Interacting with interactable task");
    }
}

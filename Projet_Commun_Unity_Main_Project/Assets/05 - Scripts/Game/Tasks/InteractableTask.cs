using UnityEngine;

public class InteractableTask : GameTask, IInteractable
{
    public void Interact()
    {
        if (Done) return;
        
        Succeed();
        Done = true;
        
        Debug.Log($"Task {TaskName} done!");
    }
}

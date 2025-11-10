using UnityEngine;

public class MopProp : InteractableProp
{
    public override void Interact(PlayerController playerController)
    {
        if(playerController.InteractableGrabbed == null) return;
        Debug.Log("Interact with mop");
    }

    public override void InteractEnd()
    {
        Debug.Log("Interact Mop End");
    }
}
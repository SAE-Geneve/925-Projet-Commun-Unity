using UnityEngine;

public class InteractableProp : Prop, IInteractable
{
    public void Interact(GameObject interactor)
    {
        Debug.Log("Interact with prop");
    }

    public override void Grabbed(PlayerController playerController)
    {
        base.Grabbed(playerController);
        playerController.InteractableGrabbed = this;
    }

    public override void Dropped(Vector3 throwForce = default, PlayerController playerController = null)
    {
        base.Dropped(throwForce, playerController);
        playerController.InteractableGrabbed = null;
    }
}

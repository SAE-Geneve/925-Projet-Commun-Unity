using UnityEngine;

public class InteractableProp : Prop, IInteractable
{
    public virtual void Interact(PlayerController playerController)
    {
        if(playerController.InteractableGrabbed == null) return;
        Debug.Log("Interact with prop");
    }

    public virtual void InteractEnd() {}

    public override void Grabbed(Controller controller)
    {
        base.Grabbed(controller);
        controller.InteractableGrabbed = this;
    }

    public override void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        base.Dropped(throwForce, controller);
        InteractEnd();
        controller.InteractableGrabbed = null;
    }
}

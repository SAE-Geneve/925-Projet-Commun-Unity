using UnityEngine;

public class InteractableProp : Prop, IInteractable
{
    public void Interact(GameObject interactor)
    {
        Debug.Log("Interact with prop");
    }

    public override void Grabbed(Controller controller)
    {
        base.Grabbed(controller);
        controller.InteractableGrabbed = this;
    }

    public override void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        base.Dropped(throwForce, controller);
        controller.InteractableGrabbed = null;
    }
}

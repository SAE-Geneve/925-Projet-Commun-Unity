using UnityEngine;

public class InteractableProp : Prop, IInteractable
{
    public string GetPromptText()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Interact(PlayerController playerController)
    {
        if(playerController.InteractableGrabbed == null) return;
        Debug.Log("Interact with prop");
    }

    public virtual void InteractEnd() {}

    public override void Grabbed(Controller controller)
    {
        controller.InteractableGrabbed = this;
        base.Grabbed(controller);
    }

    public override void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        if (Controller) Controller.InteractableGrabbed = null;
        base.Dropped(throwForce, controller);
        InteractEnd();
    }
    
}

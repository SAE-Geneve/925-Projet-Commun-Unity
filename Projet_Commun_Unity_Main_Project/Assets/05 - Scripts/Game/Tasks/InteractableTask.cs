using System;

public class InteractableTask : GameTask, IInteractable
{
    private ObjectOutline _outline;
    [NonSerialized]public PlayerController PlayerController;
    
    protected override void Start()
    {
        base.Start();

        _outline = GetComponent<ObjectOutline>();
    }

    public void Interact(PlayerController playerController)
    {
        if (Done) return;
        PlayerController = playerController;
        Succeed();
    }

    public void InteractEnd() { }
    public void AreaEnter() => _outline.EnableOutline();
    public void AreaExit() => _outline.DisableOutline();
}

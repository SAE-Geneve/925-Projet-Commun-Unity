using System;
using UnityEngine; // Ajouté au cas où pour ObjectOutline

public class InteractableTask : GameTask, IInteractable
{
    private ObjectOutline _outline;
    [NonSerialized]public PlayerController PlayerController;
    
    protected override void Start()
    {
        base.Start();

        _outline = GetComponent<ObjectOutline>();
    }

    public string GetPromptText()
    {
        throw new NotImplementedException();
    }

    public void Interact(PlayerController playerController)
    {
        if (Done) return;
        PlayerController = playerController;
        
        Succeed(playerController); 
    }

    public void InteractEnd() { }
    public void AreaEnter() => _outline.EnableOutline();
    public void AreaExit() => _outline.DisableOutline();
}
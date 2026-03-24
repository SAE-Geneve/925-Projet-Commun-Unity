public interface IInteractable
{
    string GetPromptText();
    void Interact(PlayerController playerController);
    void InteractEnd();
    void AreaEnter();
    void AreaExit();
    
}
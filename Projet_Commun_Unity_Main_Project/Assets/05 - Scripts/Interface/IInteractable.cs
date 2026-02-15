public interface IInteractable
{
    void Interact(PlayerController playerController);
    void InteractEnd();
    void AreaEnter();
    void AreaExit();
}
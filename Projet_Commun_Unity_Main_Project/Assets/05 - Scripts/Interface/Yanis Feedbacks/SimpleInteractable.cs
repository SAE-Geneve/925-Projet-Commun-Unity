using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string promptText = "Ramasser (E)";

    [Header("Audio")]
    [SerializeField] private AudioClip interactSfx;
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;

    public string GetPromptText()
    {
        return promptText;
    }

    public void Interact(PlayerController playerController)
    {
        Debug.Log("Interacted with " + gameObject.name);

        if (interactSfx != null)
        {
            AudioSource.PlayClipAtPoint(interactSfx, transform.position, volume);
        }
        else
        {
            Debug.LogWarning("Aucun interactSfx assigné sur " + gameObject.name);
        }
    }

    public void InteractEnd() { }

    public void AreaEnter() { }

    public void AreaExit() { }
}
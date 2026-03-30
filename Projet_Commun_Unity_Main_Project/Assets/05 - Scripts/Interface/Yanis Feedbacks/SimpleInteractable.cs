using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    [Header("Prompt")]
    [SerializeField] private string promptText = "Ramasser (E)";

    [Header("Audio")]
    [SerializeField] private AudioClip interactSfx;
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;

    [Header("Highlight")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] [Min(0f)] private float highlightIntensity = 2f;

    private Material _materialInstance;
    private Color _baseEmissionColor = Color.black;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        if (targetRenderer != null)
        {
            _materialInstance = targetRenderer.material;
            _materialInstance.EnableKeyword("_EMISSION");

            if (_materialInstance.HasProperty(EmissionColor))
            {
                _baseEmissionColor = _materialInstance.GetColor(EmissionColor);
            }
        }
    }

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

    public void InteractEnd()
    {
    }

    public void AreaEnter()
    {
        if (_materialInstance != null && _materialInstance.HasProperty(EmissionColor))
        {
            _materialInstance.SetColor(EmissionColor, highlightColor * highlightIntensity);
        }
    }

    public void AreaExit()
    {
        if (_materialInstance != null && _materialInstance.HasProperty(EmissionColor))
        {
            _materialInstance.SetColor(EmissionColor, _baseEmissionColor);
        }
    }
}
using System.Collections;
using UnityEngine;

public class LuggageButtonSpawner : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PropSpawner propSpawner;
    [SerializeField] private Renderer buttonRenderer;
    [SerializeField] private Material buttonPressedColor;
    
    [Header("Parameters")]
    [SerializeField] private float buttonEffectTime = 0.2f;
    
    private Coroutine _colorCoroutine;
    private Material _buttonUnpressedColor;
    private ObjectOutline _outline;

    private void Start()
    {
        _outline = GetComponent<ObjectOutline>();
        
        _buttonUnpressedColor = buttonRenderer.material;
    }

    public void Interact(PlayerController playerController)
    {
        propSpawner.SpawnProp();

        _colorCoroutine ??= StartCoroutine(ButtonColorRoutine());
    }

    public void InteractEnd()
    {

    }

    public void AreaEnter() => _outline.EnableOutline();

    public void AreaExit() => _outline.DisableOutline();

    private IEnumerator ButtonColorRoutine()
    {
        buttonRenderer.material = buttonPressedColor;
        yield return new WaitForSeconds(buttonEffectTime);
        buttonRenderer.material = _buttonUnpressedColor;
        _colorCoroutine = null;
    }
}
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    [Header("Parameters")] [SerializeField] private string outlineMaskName = "InteractOutline";
        
    private Renderer[] _renderers;
    private int _interactNumber;
    private uint _outlineMask;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _outlineMask = RenderingLayerMask.GetMask(outlineMaskName);
    }

    public void EnableOutline()
    {
        if (_interactNumber == 0)
            foreach (Renderer renderer in _renderers)
                renderer.renderingLayerMask |= _outlineMask;
        _interactNumber++;
    }

    public void DisableOutline()
    {
        if (_interactNumber == 1)
            foreach (Renderer renderer in _renderers)
                renderer.renderingLayerMask &= ~_outlineMask;
        _interactNumber--;
    }
}
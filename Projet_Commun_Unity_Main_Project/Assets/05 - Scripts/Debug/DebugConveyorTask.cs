using UnityEngine;

public class DebugConveyorTask : TriggerTask
{
    [Header("References")]
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _blueMaterial;
    
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        SwitchType();
    }

    protected override void Succeed()
    {
        base.Succeed();
        
        SwitchType();
    }

    private void SwitchType()
    {
        _propType = (PropType)Random.Range(1, 4);

        switch (_propType)
        {
            case PropType.BlueLuggage: _renderer.material = _blueMaterial; break;
            case PropType.GreenLuggage: _renderer.material = _greenMaterial; break;
            case PropType.RedLuggage: _renderer.material = _redMaterial; break;
            default: _renderer.material = _redMaterial; break;
        }
    }
}

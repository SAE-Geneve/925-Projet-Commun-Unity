using System.Collections;
using UnityEngine;

public class DebugConveyorTask : TriggerTask
{
    [Header("References")]
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _yellowMaterial;

    [Header("Parameters")] 
    [SerializeField] [Min(0.1f)] private float _minTaskDuration = 5f;
    [SerializeField] [Min(0.1f)]private float _maxTaskDuration = 15f;
    
    
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
        _propType = (PropType)Random.Range(1, 5);

        switch (_propType)
        {
            case PropType.BlueLuggage: _renderer.material = _blueMaterial; break;
            case PropType.GreenLuggage: _renderer.material = _greenMaterial; break;
            case PropType.RedLuggage: _renderer.material = _redMaterial; break;
            case PropType.YellowLuggage: _renderer.material = _yellowMaterial; break;
            default: _renderer.material = _redMaterial; break;
        }
        
        StopAllCoroutines();
        StartCoroutine(TaskCooldown());
    }

    private IEnumerator TaskCooldown()
    {
        yield return new WaitForSeconds(Random.Range(_minTaskDuration, _maxTaskDuration));
        Failed();
        SwitchType();
    }
}

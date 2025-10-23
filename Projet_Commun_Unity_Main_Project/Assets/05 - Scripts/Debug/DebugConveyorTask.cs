using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
    
    [Header("Events")]
    public UnityEvent OnReset;
    
    private Renderer _renderer;
    
    public float Timer { get; private set; }

    protected override void Start()
    {
        base.Start();
        _renderer = GetComponent<Renderer>();
        SwitchType();
    }

    protected override void Succeed()
    {
        base.Succeed();
        //After a success, cooldown to let NPC UI feedback be played without overlapping with timer
        StopAllCoroutines();
        StartCoroutine(WaitBetweenTasks());
        //SwitchType();
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
        
        Timer = Random.Range(_minTaskDuration, _maxTaskDuration);
        StartCoroutine(TaskCooldown());
        
        //Whenever the NPC receive new values, it will trigger the OnReset event and apply the appriopriate values for
        //the timer.
        OnReset.Invoke();
    }
    
    private IEnumerator WaitBetweenTasks()
    {
        //Make prop type to avoid NPC continuing receiving items between tasks
        _propType = PropType.None;
        //Waits 3 seconds before the NPC is active again
        yield return new WaitForSeconds(3f);
        SwitchType();
    }
    
    private IEnumerator TaskCooldown()
    {
        yield return new WaitForSeconds(Timer);
        Failed();
        //After a failure, cooldown to let NPC UI feedback be played without overlapping with timer
        StartCoroutine(WaitBetweenTasks());
        //SwitchType();
    }
}

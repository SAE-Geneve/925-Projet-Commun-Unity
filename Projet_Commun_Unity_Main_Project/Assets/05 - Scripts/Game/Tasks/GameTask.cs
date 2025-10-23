using UnityEngine;
using UnityEngine.Events;

public abstract class GameTask : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The name of the task")]
    [SerializeField] private string _taskName;

    [Tooltip("Does this task can be done multiple times?")]
    [SerializeField] private bool _multiple;

    [Header("Events")] 
    public UnityEvent OnStart;
    public UnityEvent OnReset;
    public UnityEvent OnSucceed;
    public UnityEvent OnFailed;

    protected bool Done;
    
    protected virtual void Start() => OnStart?.Invoke();

    protected virtual void Succeed()
    {
        if(!_multiple) Done = true;
        else OnReset?.Invoke();
        
        OnSucceed?.Invoke();
        
        Debug.Log($"Task {_taskName} done!");
    }

    protected void Failed()
    {
        if(_multiple) OnReset?.Invoke();
        
        OnFailed?.Invoke();
        
        Debug.Log($"Task {_taskName} failed!");
    }
}
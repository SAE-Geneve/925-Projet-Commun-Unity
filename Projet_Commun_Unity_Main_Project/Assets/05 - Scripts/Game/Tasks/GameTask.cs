using System;
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
    public UnityEvent OnSucceed;
    public UnityEvent OnFailed;

    public event Action OnSucceedAction;
    public event Action OnFailedAction;

    protected bool Done;
    
    protected virtual void Start() => OnStart?.Invoke();

    protected virtual void Succeed()
    {
        if(!_multiple) Done = true;
        
        OnSucceed?.Invoke();
        OnSucceedAction?.Invoke();
        
        Debug.Log($"Task {_taskName} done!");
    }

    protected void Failed()
    {
        OnFailed?.Invoke();
        OnFailedAction?.Invoke();
        
        Debug.Log($"Task {_taskName} failed!");
    }
}
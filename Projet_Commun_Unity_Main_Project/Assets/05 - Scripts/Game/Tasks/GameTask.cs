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
    public UnityEvent OnSucceed;
    public UnityEvent OnFailed;

    protected bool Done;

    protected virtual void Succeed()
    {
        if(!_multiple) Done = true;
        OnSucceed?.Invoke();
        
        Debug.Log($"Task {_taskName} done!");
    }

    protected void Failed() => OnFailed?.Invoke();
}
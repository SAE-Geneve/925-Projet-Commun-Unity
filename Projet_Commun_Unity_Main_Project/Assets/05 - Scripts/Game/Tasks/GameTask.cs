using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameTask : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The name of the task")]
    [SerializeField] private string _taskName;
    [SerializeField] private bool _finishedMission = false;

    [Tooltip("Does this task can be done multiple times?")]
    [SerializeField] private bool _multiple;
    [SerializeField] private int _multipleTaskLimit = -1;
    private int _multipleTaskCounter = 0;

    [Header("Events")] 
    public UnityEvent OnStart;
    public UnityEvent OnSucceed;
    public UnityEvent OnFailed;

    public event Action OnSucceedAction;
    public event Action OnFailedAction;

    public bool Done { get; private set; }

    protected virtual void Start() => OnStart?.Invoke();

    protected virtual void Succeed()
    {
        if (_multipleTaskLimit >= 0)
        {
            _multipleTaskCounter++;
        }
        
        if(!_multiple || _multipleTaskCounter >= _multipleTaskLimit) Done = true;
        
        OnSucceed?.Invoke();
        OnSucceedAction?.Invoke();
        
        if (_finishedMission && Done)
            GameManager.Instance.CurrentMission.Finish();
        
        Debug.Log($"Task {_taskName} done!");
    }

    protected void Failed()
    {
        OnFailed?.Invoke();
        OnFailedAction?.Invoke();
        
        Debug.Log($"Task {_taskName} failed!");
    }
}
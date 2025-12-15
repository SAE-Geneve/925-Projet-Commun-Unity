using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameTask : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The name of the task")]
    [SerializeField] private string _taskName;
    [SerializeField] private bool _destroyOnSucceed;
    [SerializeField] private bool _finishedMission;

    [Tooltip("Does this task can be done multiple times?")]
    [SerializeField] private bool _multiple;
    [SerializeField] private int _multipleTaskLimit = -1;

    [Header("Events")] 
    public UnityEvent OnStart;
    public UnityEvent OnSucceed;
    public UnityEvent OnFailed;
    public UnityEvent OnFinished;

    public event Action OnSucceedAction;
    public event Action OnFailedAction;

    public bool Done { get; private set; }
    
    private int _multipleTaskCounter;

    private bool _failed;

    protected virtual void Start() => OnStart?.Invoke();

    protected virtual void Succeed()
    {
        if(_failed) return;
        
        if (_multipleTaskLimit >= 0)
            _multipleTaskCounter++;
        
        if(!_multiple || _multipleTaskCounter == _multipleTaskLimit) Done = true;
        
        OnSucceed?.Invoke();
        OnSucceedAction?.Invoke();

        if (Done)
        {
            if(_finishedMission) GameManager.Instance.CurrentMission.Finish();
            
            OnFinished?.Invoke();
            
            Debug.Log($"Task {_taskName} done!");
            
            if(_destroyOnSucceed) Destroy(gameObject);
        }
    }

    public void Failed()
    {
        _failed = true;
        
        OnFailed?.Invoke();
        OnFailedAction?.Invoke();
        
        Debug.Log($"Task {_taskName} failed!");
    }

    public void ResetTask()
    {
        Done = false;
        
        _failed = false;
        _multipleTaskCounter = 0;
    }
}
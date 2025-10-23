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
    
    //For this mini-game, Start() doesn't work as it will trigger before the NPC even receive the proper values
    //This means that timer UI will receive the wrong values
    //Also I don't think it'll work in other mini-games due to the issue mentioned above
    private void Start() => OnStart?.Invoke();

    protected virtual void Succeed()
    {
        if(!_multiple) Done = true;
        //By calling OnReset directly, we end up starting the NPC UI timer before the NPC even gets to receive his values
        //This means that the timer UI values will always be behind the NPC values
        //else OnReset?.Invoke();
        
        OnSucceed?.Invoke();
        
        Debug.Log($"Task {_taskName} done!");
    }

    protected void Failed()
    {
        //By calling OnReset directly, we end up starting the NPC UI timer before the NPC even gets to receive his values
        //This means that the timer UI values will always be behind the NPC values
        //if(_multiple) OnReset?.Invoke();
        
        OnFailed?.Invoke();
        
        Debug.Log($"Task {_taskName} failed!");
    }
}
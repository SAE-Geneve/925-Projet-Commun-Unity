using UnityEngine;
using UnityEngine.Events;

public abstract class GameTask : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] protected string TaskName;
    
    [Header("Events")]
    public UnityEvent OnSucceed;
    public UnityEvent OnFailed;

    protected bool Done;
    
    protected void Succeed() => OnSucceed?.Invoke();
    protected void Failed() => OnFailed?.Invoke();
}
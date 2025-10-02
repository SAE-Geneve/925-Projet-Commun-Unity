using System;
using UnityEngine;

public abstract class GameTask : MonoBehaviour
{
    public event Action OnSucceed;
    public event Action OnFailed;
    
    protected void Succeed() => OnSucceed?.Invoke();
    protected void Failed() => OnFailed?.Invoke();
}
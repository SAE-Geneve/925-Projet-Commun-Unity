using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public abstract void TriggerEvent();

    public abstract void ResetEvent();

    public abstract bool IsEventActive();
}
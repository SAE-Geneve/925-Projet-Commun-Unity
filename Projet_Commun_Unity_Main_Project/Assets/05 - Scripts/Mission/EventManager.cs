using System.Collections;
using UnityEngine;

public abstract class EventManager : MonoBehaviour
{
    [Header("Global Event Settings")]
    [Tooltip("Temps avant que le tout premier événement n'arrive")]
    [SerializeField] protected float _startDelay = 15f;

    [Tooltip("Temps minimum à attendre avant le prochain événement")]
    [SerializeField] protected float _minEventInterval = 25f;

    [Tooltip("Temps maximum à attendre avant le prochain événement")]
    [SerializeField] protected float _maxEventInterval = 45f;

    private Coroutine _eventRoutine;

    protected virtual void Start()
    {
        StartEventLoop();
    }

    public void StartEventLoop()
    {
        if (_eventRoutine != null) return;
        _eventRoutine = StartCoroutine(EventLoopRoutine());
    }

    public void StopEventLoop()
    {
        if (_eventRoutine != null)
        {
            StopCoroutine(_eventRoutine);
            _eventRoutine = null;
        }
    }
    
    public virtual void ResetManager()
    {
        StopEventLoop();
    }

    private IEnumerator EventLoopRoutine()
    {
        yield return new WaitForSeconds(_startDelay);

        while (true)
        {
            TriggerRandomEvent();
            float randomWaitTime = Random.Range(_minEventInterval, _maxEventInterval);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    protected abstract void TriggerRandomEvent();
}
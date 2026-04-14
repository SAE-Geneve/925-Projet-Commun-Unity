using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EventChance
{
    public GameEvent eventScript;
    [Tooltip("Poids de cet événement (Plus c'est haut, plus ça tombe souvent)")]
    public float weight;
}
[System.Serializable]
public struct EventIntervalByPlayerCount
{
    public float minInterval;
    public float maxInterval;
}

public class EventManager : MonoBehaviour
{
    [Header("Global Event Settings")]
    [SerializeField] private float _minStartDelay = 10f;
    [SerializeField] private float _maxStartDelay = 20f;

    [Header("Event Intervals by Player Count")]
    [Tooltip("Index 0 = 1 joueur, Index 1 = 2 joueurs, etc.")]
    [SerializeField] private EventIntervalByPlayerCount[] _intervalsByPlayerCount = new EventIntervalByPlayerCount[]
    {
        new EventIntervalByPlayerCount { minInterval = 20f, maxInterval = 30f },
        new EventIntervalByPlayerCount { minInterval = 15f, maxInterval = 25f },
        new EventIntervalByPlayerCount { minInterval = 15f, maxInterval = 20f },
        new EventIntervalByPlayerCount { minInterval = 10f, maxInterval = 15f },
    };

    [Header("Event List")]
    [SerializeField] private List<EventChance> _availableEvents;

    private Coroutine _eventRoutine;

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
        foreach (var ev in _availableEvents)
            if (ev.eventScript != null) ev.eventScript.ResetEvent();
    }

    private IEnumerator EventLoopRoutine()
    {
        yield return new WaitForSeconds(Random.Range(_minStartDelay, _maxStartDelay));

        while (true)
        {
            TriggerRandomEvent();
            yield return new WaitForSeconds(GetRandomInterval());
        }
    }

    private float GetRandomInterval()
    {
        int playerCount = PlayerManager.Instance != null ? PlayerManager.Instance.PlayerCount : 1;
        int index = Mathf.Clamp(playerCount - 1, 0, _intervalsByPlayerCount.Length - 1);
        var interval = _intervalsByPlayerCount[index];
        return Random.Range(interval.minInterval, interval.maxInterval);
    }

    private void TriggerRandomEvent()
    {
        if (_availableEvents.Count == 0) return;

        float totalWeight = 0f;
        List<EventChance> possibleEvents = new List<EventChance>();

        foreach (var ev in _availableEvents)
        {
            if (ev.eventScript != null && !ev.eventScript.IsEventActive())
            {
                totalWeight += ev.weight;
                possibleEvents.Add(ev);
            }
        }

        if (possibleEvents.Count == 0) return;

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var ev in possibleEvents)
        {
            currentWeight += ev.weight;
            if (randomValue <= currentWeight)
            {
                ev.eventScript.TriggerEvent();
                break;
            }
        }
    }
}
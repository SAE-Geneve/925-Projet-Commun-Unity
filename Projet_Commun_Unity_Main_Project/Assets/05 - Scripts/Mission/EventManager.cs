using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// La petite structure pour ta liste dans l'inspecteur
[System.Serializable]
public struct EventChance
{
    public GameEvent eventScript;
    [Tooltip("Poids de cet événement (Plus c'est haut, plus ça tombe souvent)")]
    public float weight;
}

public class EventManager : MonoBehaviour
{
    [Header("Global Event Settings")]
    [SerializeField] protected float _startDelay = 15f;
    [SerializeField] protected float _minEventInterval = 25f;
    [SerializeField] protected float _maxEventInterval = 45f;

    [Header("Event List")]
    [Tooltip("Ajoute ici tous les scripts d'événements et leurs probabilités !")]
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
        {
            if (ev.eventScript != null) ev.eventScript.ResetEvent();
        }
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

    private void TriggerRandomEvent()
    {
        if (_availableEvents.Count == 0) return;

        // 1. On calcule le total des probabilités des événements qui NE SONT PAS déjà actifs
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

        // Si tout est déjà en cours, on annule pour ce tour
        if (possibleEvents.Count == 0) return;

        // 2. On tire au sort !
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var ev in possibleEvents)
        {
            currentWeight += ev.weight;
            if (randomValue <= currentWeight)
            {
                // ON A UN GAGNANT ! On lance l'événement
                ev.eventScript.TriggerEvent();
                break;
            }
        }
    }
}
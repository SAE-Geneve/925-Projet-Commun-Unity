using System.Collections.Generic;
using UnityEngine;

public class PuddleSpawnEvent : GameEvent
{
    [Header("Puddle Settings")]
    [SerializeField] private PuddleTask _puddlePrefab;
    [SerializeField] private List<Transform> _puddleSpawnPoints;
    [SerializeField] private int _maxConcurrentPuddles = 3;
    [SerializeField] private int _minPuddlesPerEvent = 2; 
    [SerializeField] private int _maxPuddlesPerEvent = 3; 

    [Header("Prop Reset Settings")]
    [SerializeField] private List<Prop> _mopsToReset;

    private List<Transform> _availableSpawnPoints = new();
    private Dictionary<PuddleTask, Transform> _activePuddlesMap = new();

    private void Start()
    {
        _availableSpawnPoints.AddRange(_puddleSpawnPoints);
    }

    public override bool IsEventActive()
    {
        return _activePuddlesMap.Count >= _maxConcurrentPuddles || _availableSpawnPoints.Count == 0;
    }

    public override void TriggerEvent()
    {
        if (_activePuddlesMap.Count >= _maxConcurrentPuddles || _availableSpawnPoints.Count == 0) return;

        int puddlesToSpawn = Random.Range(_minPuddlesPerEvent, _maxPuddlesPerEvent + 1);

        for (int i = 0; i < puddlesToSpawn; i++)
        {
            if (_activePuddlesMap.Count >= _maxConcurrentPuddles || _availableSpawnPoints.Count == 0) 
                break;

            int randomIndex = Random.Range(0, _availableSpawnPoints.Count);
            Transform spawnPoint = _availableSpawnPoints[randomIndex];
            _availableSpawnPoints.RemoveAt(randomIndex);

            PuddleTask newPuddle = Instantiate(_puddlePrefab, spawnPoint.position, spawnPoint.rotation);

            _activePuddlesMap.Add(newPuddle, spawnPoint);
            newPuddle.OnSucceedWithPlayer += HandlePuddleCleaned;
        }
    }

    private void HandlePuddleCleaned(PlayerController player)
    {
        PuddleTask cleanedPuddle = null;
        foreach (var kvp in _activePuddlesMap)
        {
            if (kvp.Key.Done)
            {
                cleanedPuddle = kvp.Key;
                _availableSpawnPoints.Add(kvp.Value);
                break;
            }
        }

        if (cleanedPuddle != null)
        {
            cleanedPuddle.OnSucceedWithPlayer -= HandlePuddleCleaned;
            _activePuddlesMap.Remove(cleanedPuddle);
        }

        RewardPlayer(player);
    }

    public override void ResetEvent()
    {
        foreach (var kvp in _activePuddlesMap)
        {
            if (kvp.Key != null)
            {
                kvp.Key.OnSucceedWithPlayer -= HandlePuddleCleaned;
                Destroy(kvp.Key.gameObject);
            }
        }
        _activePuddlesMap.Clear();

        _availableSpawnPoints.Clear();
        _availableSpawnPoints.AddRange(_puddleSpawnPoints);

        foreach (var mop in _mopsToReset)
        {
            if (mop != null) mop.Respawn();
        }
    }
}
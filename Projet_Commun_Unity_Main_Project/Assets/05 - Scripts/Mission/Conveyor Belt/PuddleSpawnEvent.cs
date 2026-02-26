using System.Collections.Generic;
using UnityEngine;

public class PuddleSpawnEvent : GameEvent
{
    [Header("Puddle Settings")]
    [SerializeField] private PuddleTask _puddlePrefab;
    [SerializeField] private List<Transform> _puddleSpawnPoints;
    [SerializeField] private int _maxConcurrentPuddles = 3;

    [Header("Prop Reset Settings")]
    [SerializeField] private List<Prop> _mopsToReset;

    private List<Transform> _availableSpawnPoints = new();
    private List<PuddleTask> _activePuddlesList = new();

    private void Start()
    {
        _availableSpawnPoints.AddRange(_puddleSpawnPoints);
    }

    public override bool IsEventActive()
    {
        // Si on a atteint la limite de 3 flaques, le Manager ne pourra plus piocher cet évent !
        return _activePuddlesList.Count >= _maxConcurrentPuddles || _availableSpawnPoints.Count == 0;
    }

    public override void TriggerEvent()
    {
        // Double sécurité
        if (_activePuddlesList.Count >= _maxConcurrentPuddles || _availableSpawnPoints.Count == 0) return;

        Debug.Log("EVENT: Une nouvelle flaque apparaît !");

        int randomIndex = Random.Range(0, _availableSpawnPoints.Count);
        Transform spawnPoint = _availableSpawnPoints[randomIndex];
        _availableSpawnPoints.RemoveAt(randomIndex);

        PuddleTask newPuddle = Instantiate(_puddlePrefab, spawnPoint.position, spawnPoint.rotation);
        _activePuddlesList.Add(newPuddle);

        // On abonne la flaque pour qu'elle se retire de la liste quand elle est nettoyée
        newPuddle.OnSucceedAction += () => OnPuddleCleaned(spawnPoint, newPuddle);
    }

    private void OnPuddleCleaned(Transform freedSpawnPoint, PuddleTask puddle)
    {
        _activePuddlesList.Remove(puddle);
        _availableSpawnPoints.Add(freedSpawnPoint);
    }

    public override void ResetEvent()
    {
        foreach (var puddle in _activePuddlesList)
        {
            if (puddle != null) Destroy(puddle.gameObject);
        }
        _activePuddlesList.Clear();
        
        _availableSpawnPoints.Clear();
        _availableSpawnPoints.AddRange(_puddleSpawnPoints);

        foreach (var mop in _mopsToReset)
        {
            if (mop != null) mop.Respawn();
        }
    }
}
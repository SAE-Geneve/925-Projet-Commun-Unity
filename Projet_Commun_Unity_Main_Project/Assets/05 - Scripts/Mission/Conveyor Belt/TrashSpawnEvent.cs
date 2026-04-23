using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnEvent : GameEvent
{
    [Header("Trash Settings")]
    [SerializeField] private GameObject _trashPrefab;
    [SerializeField] private List<Transform> _trashSpawnPoints;
    [SerializeField] private int _maxConcurrentTrashs = 15;
    [SerializeField] private int _minTrashsPerEvent = 2; 
    [SerializeField] private int _maxTrashsPerEvent = 3; 
    [SerializeField] private float _spawnOffset = 0.5f;

    private List<Transform> _unusedSpawnPoints = new();
    private List<GameObject> _activeTrashs = new();
    private Transform _lastSpawnPoint;

    private void Start()
    {
        _unusedSpawnPoints.AddRange(_trashSpawnPoints);
    }

    public override bool IsEventActive()
    {
        return _activeTrashs.Count >= _maxConcurrentTrashs;
    }

    public override void TriggerEvent()
    {
        if (_activeTrashs.Count >= _maxConcurrentTrashs) return;

        int trashsToSpawn = Random.Range(_minTrashsPerEvent, _maxTrashsPerEvent + 1);

        for (int i = 0; i < trashsToSpawn; i++)
        {
            if (_activeTrashs.Count >= _maxConcurrentTrashs) 
                break;

            if (_unusedSpawnPoints.Count == 0)
            {
                _unusedSpawnPoints.AddRange(_trashSpawnPoints);
            }

            int randomIndex = Random.Range(0, _unusedSpawnPoints.Count);

            if (_unusedSpawnPoints[randomIndex] == _lastSpawnPoint && _unusedSpawnPoints.Count > 1)
            {
                randomIndex = (randomIndex + 1) % _unusedSpawnPoints.Count;
            }

            Transform spawnPoint = _unusedSpawnPoints[randomIndex];
            _lastSpawnPoint = spawnPoint;
            _unusedSpawnPoints.RemoveAt(randomIndex);

            Vector3 randomOffset = new Vector3(
                Random.Range(-_spawnOffset, _spawnOffset),
                0f,
                Random.Range(-_spawnOffset, _spawnOffset)
            );

            GameObject newTrash = Instantiate(_trashPrefab, spawnPoint.position + randomOffset, spawnPoint.rotation);
            _activeTrashs.Add(newTrash);
        }
    }

    public override void ResetEvent()
    {
        foreach (GameObject trash in _activeTrashs)
        {
            if (trash != null) Destroy(trash);
        }
        _activeTrashs.Clear();

        _unusedSpawnPoints.Clear();
        _unusedSpawnPoints.AddRange(_trashSpawnPoints);
        _lastSpawnPoint = null;
    }
}
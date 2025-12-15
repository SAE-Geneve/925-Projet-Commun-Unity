using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] protected List<Transform> spawnPoints;
    [SerializeField] protected AIController npcPrefab;
    [SerializeField] protected float spawnInterval = 5f;
    [SerializeField] private bool _spawnOnStart;

    // [Header("Debug / Test")]
    // [SerializeField] protected bool spawnEnabled;

    private Coroutine _spawnRoutine;

    private void Update()
    {
        // Détecte changement de checkbox
        // if (spawnEnabled && _spawnRoutine == null)
        //     StartSpawn();
        // else if (!spawnEnabled && _spawnRoutine != null)
        //     StopSpawn();
    }

    protected virtual void Start()
    {
        if(_spawnOnStart) StartSpawn();
    }

    public void StartSpawn()
    {
        if (_spawnRoutine != null) return;
        _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnNPC();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    protected virtual void SpawnNPC()
    {
        if (npcPrefab || spawnPoints == null || spawnPoints.Count == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
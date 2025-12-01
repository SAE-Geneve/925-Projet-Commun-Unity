using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] protected List<Transform> spawnPoints;
    [SerializeField] protected AIController npcPrefab;
    [SerializeField] protected float spawnInterval = 5f;

    [Header("Debug / Test")]
    [SerializeField] protected bool spawnEnabled = false;

    private Coroutine spawnRoutine;

    private void Update()
    {
        // Détecte changement de checkbox
        if (spawnEnabled && spawnRoutine == null)
            StartSpawn();
        else if (!spawnEnabled && spawnRoutine != null)
            StopSpawn();
    }

    public void StartSpawn()
    {
        if (spawnRoutine != null) return;
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawn()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
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
        if (spawnPoints == null || spawnPoints.Count == 0 || npcPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
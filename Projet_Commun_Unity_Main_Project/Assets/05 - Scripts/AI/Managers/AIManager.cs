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

    [Header("Spawn Limit")]
    [SerializeField] protected bool useSpawnLimit = false;
    [SerializeField] protected int maxSpawnAmount = 10;

    private Coroutine _spawnRoutine;
    
    protected readonly List<AIController> _spawnedAIs = new();

    protected virtual void Start()
    {
        if(_spawnOnStart) StartSpawn();
    }

    public void StartSpawn()
    {
        if (_spawnRoutine != null) return;
        _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public virtual void StopSpawn()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }
    
    public void ClearAIs()
    {
        StopSpawn();

        for (int i = _spawnedAIs.Count - 1; i >= 0; i--)
        {
            var ai = _spawnedAIs[i];
            if (!ai) continue;
            
            ai.DestroyAI();
        }

        _spawnedAIs.Clear();
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!useSpawnLimit || _spawnedAIs.Count < maxSpawnAmount)
            {
                SpawnNPC();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    protected virtual void SpawnNPC()
    {
        if (!npcPrefab || spawnPoints == null || spawnPoints.Count == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController newNpc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        
        newNpc.OnDestroyed += RemoveAI;
        
        _spawnedAIs.Add(newNpc);
    }
    
    protected virtual void RemoveAI(AIController ai)
    {
        ai.OnDestroyed -= RemoveAI;
        _spawnedAIs.Remove(ai);
    }
}
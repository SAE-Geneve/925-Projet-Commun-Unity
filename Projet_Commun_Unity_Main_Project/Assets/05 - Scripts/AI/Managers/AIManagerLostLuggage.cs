using UnityEngine;

public class AIManagerLostLuggage : AIManager
{
    protected override void SpawnNPC()
    {
        if (!npcPrefab || spawnPoints == null || spawnPoints.Count == 0) return;
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            AIController newNpc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            
            newNpc.OnDestroyed += RemoveAI;
            
            _spawnedAIs.Add(newNpc);
        }
        
        StopSpawn();
    }
}
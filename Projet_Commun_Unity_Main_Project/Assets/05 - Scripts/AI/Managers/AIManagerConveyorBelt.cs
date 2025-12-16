using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AIManagerConveyorBelt : AIManager
{
    [Header("Conveyor Belt Settings")]
    [SerializeField] private List<LocationPoint> locations;
    [SerializeField] private Transform exitPoint;

    protected override void Start()
    {
        base.Start();
        int playerCount = PlayerManager.Instance.Players.Count;
        int half = locations.Count / 2;
        
        if (playerCount >= 2)
        {
            for (int i = half; i < locations.Count; i++)
            {
                locations[i].available = true;
            }
        }
    }

    protected override void SpawnNPC()
    {
        if (!npcPrefab || locations == null || locations.Count == 0)
            return;

        LocationPoint chosenLocation = null;

        foreach (var loc in locations)
        {
            if (loc.available)
            {
                chosenLocation = loc;
                break;
            }
        }

        if (chosenLocation == null)
            return;

        chosenLocation.available = false;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        _spawnedAIs.Add(npc);
        
        if (!npc)
        {
            Debug.LogError("Le prefab n'est pas un AIConveyorBelt.");
            return;
        }

        BehaviorGraphAgent agent = npc.BehaviorAgent;
        
        if (!agent) return;
        if (exitPoint) agent.SetVariableValue("Exit", exitPoint);
        if (chosenLocation.gameObject) agent.SetVariableValue("Location", chosenLocation.gameObject);
    }

    public override void StopSpawn()
    {
        base.StopSpawn();
        foreach (var location in locations)
            location.available = true;
    }
}
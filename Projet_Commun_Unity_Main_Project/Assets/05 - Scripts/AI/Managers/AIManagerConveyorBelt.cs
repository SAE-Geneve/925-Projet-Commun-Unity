using System.Collections.Generic;
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
        if (npcPrefab == null || locations == null || locations.Count == 0)
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
        AIConveyorBelt npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation) as AIConveyorBelt;

        if (npc == null)
        {
            Debug.LogError("Le prefab n'est pas un AIConveyorBelt.");
            return;
        }

        npc.Initialize(chosenLocation.locationObject, exitPoint);
    }
}
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AIManagerBorder : AIManager
{
    [Header("Border Game Objects")]
    [SerializeField] private Transform conveyor;
    [SerializeField] private Transform scanZone;
    [SerializeField] private Transform throwHere;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform rejectedExitPoint;
    [SerializeField] private PropManager propManager;
    
    [SerializeField] private InteractableTask YesTask;
    [SerializeField] private InteractableTask NoTask;
    [SerializeField] private LocationPoint[] waitPos;

    [Header("Spawning Settings")]
    [SerializeField] private float spawnCooldown = 1.5f; 
    [SerializeField] private float timeToReachQueue = 4f;
    
    private float _lastSpawnTime;
    private List<float> _npcInTransitTimes = new List<float>();

    protected override void SpawnNPC()
    {
        if (spawnPoints == null || spawnPoints.Count == 0 || !npcPrefab) return;
        if (waitPos == null || waitPos.Length == 0) return;
        
        if (Time.time - _lastSpawnTime < spawnCooldown) return;
        
        _npcInTransitTimes.RemoveAll(time => Time.time - time > timeToReachQueue);
        
        int freeSpots = 0;
        foreach (var point in waitPos)
        {
            if (point != null && point.available)
            {
                freeSpots++;
            }
        }
        
        if (freeSpots <= _npcInTransitTimes.Count) return;
        
        _lastSpawnTime = Time.time;
        _npcInTransitTimes.Add(Time.time);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        
        _spawnedAIs.Add(npc); 

        BehaviorGraphAgent agent = npc.BehaviorAgent;

        if (agent)
        {
            List<GameObject> locationObjects = new List<GameObject>();
            foreach (var point in waitPos)
            {
                if (point != null) 
                {
                    locationObjects.Add(point.gameObject);
                }
            }
            agent.SetVariableValue("ConveyorBelt", conveyor);
            agent.SetVariableValue("Detector", scanZone);
            agent.SetVariableValue("ThrowHere", throwHere);
            agent.SetVariableValue("Exit", exitPoint);
            agent.SetVariableValue("RejectedExit", rejectedExitPoint);
            agent.SetVariableValue("PropManager", propManager);
            
            agent.SetVariableValue("Locations", locationObjects);
            agent.SetVariableValue("AcceptedTask", YesTask);
            agent.SetVariableValue("RejectedTask", NoTask);
        }
    }

    public void ResetLocations()
    {
        foreach (LocationPoint loc in waitPos)
        {
            if (loc != null)
            {
                loc.available = true;
            }
        }
    }
}
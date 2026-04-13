using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.UI;

public class AIManagerBorder : AIManager
{
    [Header("Border Game Objects (Shared)")]
    [SerializeField] private Transform conveyor;
    [SerializeField] private Transform scanZone;
    [SerializeField] private Transform throwHere;
    [SerializeField] private PropManager propManager;

    [System.Serializable]
    public struct BorderQueue
    {
        public LocationPoint[] locations;
        public Transform exitPoint;
        public Transform rejectedExitPoint;
        public InteractableTask yesTask;
        public InteractableTask noTask;
        public RenderTexture cctvTexture;
    }

    [Header("Queues")]
    [SerializeField] private BorderQueue queue1;
    [SerializeField] private BorderQueue queue2;

    [Header("Spawning Settings")]
    [SerializeField] private float spawnCooldown = 1.5f;
    [SerializeField] private float timeToReachQueue = 4f;
    
    private float _lastSpawnTime;
    private List<float> _npcInTransitTimes = new List<float>();
    
    [HideInInspector] public bool isSpawningPaused = false;
    
    protected override void AdaptToPlayerCount()
    {
        base.AdaptToPlayerCount();
        
        if (PlayerManager.Instance != null)
        {
            int playerCount = PlayerManager.Instance.PlayerCount;
            float multiplier = 1f + (playerCount - 1) * 0.5f;
            spawnCooldown = spawnCooldown / multiplier;
        }
    }

    protected override void SpawnNPC()
    {
        if (isSpawningPaused) return;
        if (spawnPoints == null || spawnPoints.Count == 0 || !npcPrefab) return;
        if (Time.time - _lastSpawnTime < spawnCooldown) return;

        BorderQueue chosenQueue = GetShortestQueue(out bool isQueue1);
        if (chosenQueue.locations == null || chosenQueue.locations.Length == 0) return;
        
        for (int i = _npcInTransitTimes.Count - 1; i >= 0; i--)
        {
            if (Time.time - _npcInTransitTimes[i] > timeToReachQueue)
                _npcInTransitTimes.RemoveAt(i);
        }

        int freeSpots = 0;
        foreach (var point in chosenQueue.locations)
        {
            if (point != null && point.available) freeSpots++;
        }

        if (freeSpots <= _npcInTransitTimes.Count) return;

        _lastSpawnTime = Time.time;
        _npcInTransitTimes.Add(Time.time);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        _spawnedAIs.Add(npc);
        
        if (npc.cctvRawImage != null && chosenQueue.cctvTexture != null)
        {
            npc.cctvRawImage.texture = chosenQueue.cctvTexture;
        }

        BehaviorGraphAgent agent = npc.BehaviorAgent;
        if (agent)
        {
            List<GameObject> locationObjects = new List<GameObject>();
            foreach (var point in chosenQueue.locations)
            {
                if (point != null) locationObjects.Add(point.gameObject);
            }

            agent.SetVariableValue("ConveyorBelt", conveyor);
            agent.SetVariableValue("Detector", scanZone);
            agent.SetVariableValue("ThrowHere", throwHere);
            agent.SetVariableValue("Exit", chosenQueue.exitPoint);
            agent.SetVariableValue("RejectedExit", chosenQueue.rejectedExitPoint);
            agent.SetVariableValue("PropManager", propManager);
            agent.SetVariableValue("Locations", locationObjects);
            agent.SetVariableValue("AcceptedTask", chosenQueue.yesTask);
            agent.SetVariableValue("RejectedTask", chosenQueue.noTask);
        }
    }

    private BorderQueue GetShortestQueue(out bool isQueue1)
    {
        bool q1Valid = queue1.locations != null && queue1.locations.Length > 0;
        bool q2Valid = queue2.locations != null && queue2.locations.Length > 0;

        if (!q1Valid && !q2Valid) { isQueue1 = true; return queue1; }
        if (!q1Valid) { isQueue1 = false; return queue2; }
        if (!q2Valid) { isQueue1 = true; return queue1; }

        isQueue1 = CountOccupied(queue1.locations) <= CountOccupied(queue2.locations);
        return isQueue1 ? queue1 : queue2;
    }

    private int CountOccupied(LocationPoint[] locations)
    {
        int count = 0;
        foreach (var point in locations)
        {
            if (point != null && !point.available) count++;
        }
        return count;
    }

    public void ResetLocations()
    {
        if (queue1.locations != null)
            foreach (var loc in queue1.locations) if (loc != null) loc.available = true;
        if (queue2.locations != null)
            foreach (var loc in queue2.locations) if (loc != null) loc.available = true;
    }
}
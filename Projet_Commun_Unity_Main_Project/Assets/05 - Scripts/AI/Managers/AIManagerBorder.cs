using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AIManagerBorder : AIManager
{
    [Header("Border Game Objects (Shared)")]
    [SerializeField] private Transform conveyor;
    [SerializeField] private Transform scanZone;
    [SerializeField] private Transform throwHere;
    [SerializeField] private PropManager propManager;

    [System.Serializable]
    private struct BorderQueue
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
    [SerializeField] private float timeToReachQueue = 4f;

    [Header("Spawn Cooldown by Player Count")]
    [Tooltip("Index 0 = 1 joueur, Index 1 = 2 joueurs, etc.")]
    [SerializeField] private float[] minSpawnCooldowns = { 10f, 8f, 7f, 5f };
    [SerializeField] private float[] maxSpawnCooldowns = { 15f, 13f, 11f, 10f };

    private float _lastSpawnTime;
    private float _currentSpawnCooldown;
    private List<float> _npcInTransitTimes = new List<float>();

    [HideInInspector] public bool isSpawningPaused = false;

    protected override void AdaptToPlayerCount()
    {
        base.AdaptToPlayerCount();
        RollNewCooldown();
    }

    private void RollNewCooldown()
    {
        int playerCount = PlayerManager.Instance != null ? PlayerManager.Instance.PlayerCount : 1;
        int index = Mathf.Clamp(playerCount - 1, 0, minSpawnCooldowns.Length - 1);
        _currentSpawnCooldown = Random.Range(minSpawnCooldowns[index], maxSpawnCooldowns[index]);
        Debug.Log($"[AIManagerBorder] Joueurs: {playerCount} | Prochain spawn dans: {_currentSpawnCooldown:F1}s");
    }

    protected override void SpawnNPC()
    {
        if (isSpawningPaused) return;
        if (spawnPoints == null || spawnPoints.Count == 0 || !npcPrefab) return;
        if (Time.time - _lastSpawnTime < _currentSpawnCooldown) return;

        BorderQueue chosenQueue = GetShortestQueue();
        if (chosenQueue.locations == null || chosenQueue.locations.Length == 0) return;

        CleanTransitTimes();
        if (!HasFreeSpot(chosenQueue)) return;

        _lastSpawnTime = Time.time;
        _npcInTransitTimes.Add(Time.time);
        RollNewCooldown();

        AIController npc = InstantiateNPC();
        ApplyCCTV(npc, chosenQueue);
        ApplyBlackboard(npc, chosenQueue);
    }

    protected override void Start()
    {
        base.Start();
        _lastSpawnTime = -999f;
    }

    private void CleanTransitTimes()
    {
        for (int i = _npcInTransitTimes.Count - 1; i >= 0; i--)
            if (Time.time - _npcInTransitTimes[i] > timeToReachQueue)
                _npcInTransitTimes.RemoveAt(i);
    }

    private bool HasFreeSpot(BorderQueue queue)
    {
        int freeSpots = 0;
        foreach (var point in queue.locations)
            if (point != null && point.available) freeSpots++;
        return freeSpots > _npcInTransitTimes.Count;
    }

    private AIController InstantiateNPC()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        _spawnedAIs.Add(npc);
        return npc;
    }

    private void ApplyCCTV(AIController npc, BorderQueue queue)
    {
        if (npc.CctvRawImage && queue.cctvTexture)
            npc.CctvRawImage.texture = queue.cctvTexture;
    }

    private void ApplyBlackboard(AIController npc, BorderQueue queue)
    {
        BehaviorGraphAgent agent = npc.BehaviorAgent;
        if (!agent) return;

        List<GameObject> locationObjects = new List<GameObject>();
        foreach (var point in queue.locations)
            if (point != null) locationObjects.Add(point.gameObject);

        agent.SetVariableValue("ConveyorBelt", conveyor);
        agent.SetVariableValue("Detector", scanZone);
        agent.SetVariableValue("ThrowHere", throwHere);
        agent.SetVariableValue("Exit", queue.exitPoint);
        agent.SetVariableValue("RejectedExit", queue.rejectedExitPoint);
        agent.SetVariableValue("PropManager", propManager);
        agent.SetVariableValue("Locations", locationObjects);
        agent.SetVariableValue("AcceptedTask", queue.yesTask);
        agent.SetVariableValue("RejectedTask", queue.noTask);
    }

    private BorderQueue GetShortestQueue()
    {
        bool q1Valid = queue1.locations != null && queue1.locations.Length > 0;
        bool q2Valid = queue2.locations != null && queue2.locations.Length > 0;

        if (!q1Valid && !q2Valid) return queue1;
        if (!q1Valid) return queue2;
        if (!q2Valid) return queue1;

        return CountOccupied(queue1.locations) <= CountOccupied(queue2.locations) ? queue1 : queue2;
    }

    private int CountOccupied(LocationPoint[] locations)
    {
        int count = 0;
        foreach (var point in locations)
            if (point != null && !point.available) count++;
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
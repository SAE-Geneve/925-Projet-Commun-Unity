using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;

public class AIManagerConveyorBelt : AIManager
{
    [Header("Conveyor Belt Settings")]
    [SerializeField] private List<LocationPoint> locations;
    [SerializeField] private Transform exitPoint;

    [Header("Events")]
    [SerializeField] private UnityEvent onSucceed;
    [SerializeField] private UnityEvent onFailed;

    protected override void Start()
    {
        base.Start();
        if (PlayerManager.Instance != null)
        {
            int playerCount = PlayerManager.Instance.Players.Count;
            int half = locations.Count / 2;
            
            if (playerCount >= 2)
            {
                for (int i = half; i < locations.Count; i++)
                    locations[i].available = true;
            }
        }
    }

    protected override void SpawnNPC()
    {
        if (npcPrefab == null || locations == null || locations.Count == 0)
            return;
        
        List<LocationPoint> availableSpots = new List<LocationPoint>();

        foreach (var loc in locations)
        {
            if (loc.available)
            {
                availableSpots.Add(loc);
            }
        }
        
        if (availableSpots.Count == 0) return;
        LocationPoint chosenLocation = availableSpots[Random.Range(0, availableSpots.Count)];
        chosenLocation.available = false;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        
        _spawnedAIs.Add(npc);
        
        if (!npc)
        {
            Debug.LogError("L'IA instanciée est nulle.");
            return;
        }
        
        if(!npc.GameTask) return;

        npc.GameTask.OnSucceedAction += Succeed;
        npc.GameTask.OnFailedAction += Failed;
        npc.OnDestroyed += RemoveAI;
        
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

    protected override void RemoveAI(AIController ai)
    {
        if (ai && ai.GameTask)
        {
            ai.GameTask.OnSucceedAction -= Succeed;
            ai.GameTask.OnFailedAction -= Failed;
        }
        base.RemoveAI(ai);
    }
    
    private void Succeed() => onSucceed?.Invoke();
    private void Failed() => onFailed?.Invoke();
}
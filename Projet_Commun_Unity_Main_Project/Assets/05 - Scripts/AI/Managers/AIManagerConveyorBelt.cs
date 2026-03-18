using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;

public class AIManagerConveyorBelt : AIManager
{
    [Header("Conveyor Belt Settings")]
    [SerializeField] private List<LocationPoint> locations;
    [SerializeField] private Transform exitPoint;

    [Header("Conveyor Belt Spawners")]
    [SerializeField] private List<OnePropSpawner> propSpawners;
    [SerializeField] [Range(0f, 1f)] private float targetedSpawnChance = 0.5f;

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
        if (npcPrefab == null || locations == null || locations.Count == 0) return;

        List<LocationPoint> availableSpots = new List<LocationPoint>();
        foreach (var loc in locations)
            if (loc.available) availableSpots.Add(loc);

        if (availableSpots.Count == 0) return;

        LocationPoint chosenLocation = availableSpots[Random.Range(0, availableSpots.Count)];
        chosenLocation.available = false;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        _spawnedAIs.Add(npc);

        if (!npc.GameTask) return;

        npc.GameTask.OnSucceedAction += Succeed;
        npc.GameTask.OnFailedAction += Failed;
        npc.OnDestroyed += RemoveAI;

        BehaviorGraphAgent agent = npc.BehaviorAgent;
        if (!agent) return;
        if (exitPoint) agent.SetVariableValue("Exit", exitPoint);
        if (chosenLocation.gameObject) agent.SetVariableValue("Location", chosenLocation.gameObject);
        
        foreach (var spawner in propSpawners)
            TrySpawnTargetedLuggage(spawner);
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

    private void TrySpawnTargetedLuggage(OnePropSpawner spawner)
    {
        if (Random.value > targetedSpawnChance) return;

        PropType? targetType = GetUnsatisfiedAIType();
        spawner.SetNextTargetedType(targetType);
    }

    private PropType? GetUnsatisfiedAIType()
    {
        List<PropTypeBlackBoard> unsatisfiedTypes = new List<PropTypeBlackBoard>();

        foreach (var ai in _spawnedAIs)
        {
            if (ai == null) continue;

            BehaviorGraphAgent agent = ai.BehaviorAgent;
            if (agent == null) continue;

            if (agent.GetVariable("isLeaving", out BlackboardVariable<bool> isLeaving) && isLeaving.Value)
                continue;

            if (agent.GetVariable("PreferredType", out BlackboardVariable<PropTypeBlackBoard> preferred))
                unsatisfiedTypes.Add(preferred.Value);
        }

        if (unsatisfiedTypes.Count == 0) return null;

        PropTypeBlackBoard chosen = unsatisfiedTypes[Random.Range(0, unsatisfiedTypes.Count)];

        return chosen switch
        {
            PropTypeBlackBoard.RedLuggage    => PropType.RedLuggage,
            PropTypeBlackBoard.BlueLuggage   => PropType.BlueLuggage,
            PropTypeBlackBoard.GreenLuggage  => PropType.GreenLuggage,
            PropTypeBlackBoard.YellowLuggage => PropType.YellowLuggage,
            _ => null
        };
    }

    private void Succeed() => onSucceed?.Invoke();
    private void Failed() => onFailed?.Invoke();
}
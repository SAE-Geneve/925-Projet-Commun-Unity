using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AIManagerBoarding : AIManager
{
    [Header("Border Game Objects")]
    [SerializeField] private InteractableTask YesTask;
    [SerializeField] private InteractableTask NoTask;
    [SerializeField] private Transform badExit;
    [SerializeField] private Transform goodExit;
    [SerializeField] private LocationPoint[] waitPos;

    protected override void SpawnNPC()
    {
        if (spawnPoints == null || spawnPoints.Count == 0 || !npcPrefab) return;
        if (waitPos == null || waitPos.Length == 0) return;
        
        if (!waitPos[0].available) 
        {
            return; 
        }
        
        if (spawnPoints == null || spawnPoints.Count == 0 || !npcPrefab) return;

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
            agent.SetVariableValue("Locations", locationObjects);
            agent.SetVariableValue("ExitBad", badExit);
            agent.SetVariableValue("ExitGood", goodExit);
            agent.SetVariableValue("TaskAccepted", YesTask);
            agent.SetVariableValue("TaskRejected", NoTask);
        }
    }

    public void ResetLocations()
    {
        foreach (LocationPoint loc in waitPos)
        {
            loc.available = true;
        }
    }
    
}
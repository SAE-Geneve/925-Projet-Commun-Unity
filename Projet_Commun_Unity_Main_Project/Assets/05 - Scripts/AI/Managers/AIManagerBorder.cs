using Unity.Behavior;
using UnityEngine;

public class AIManagerBorder : AIManager
{
    [Header("Border Game Objects")]
    [SerializeField] private Transform conveyor;
    [SerializeField] private Transform scanZone;
    [SerializeField] private Transform throwHere;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private PropManager propManager;

   protected override void SpawnNPC()
   {
       if (spawnPoints == null || spawnPoints.Count == 0 || !npcPrefab) return;
   
       Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
       AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

       BehaviorGraphAgent agent = npc.BehaviorAgent;

       if (agent)
       {
           agent.SetVariableValue("ConveyorBelt", conveyor);
           agent.SetVariableValue("Detector", scanZone);
           agent.SetVariableValue("ThrowHere", throwHere);
           agent.SetVariableValue("Exit", exitPoint);
           agent.SetVariableValue("PropManager", propManager);
       }

   }
}
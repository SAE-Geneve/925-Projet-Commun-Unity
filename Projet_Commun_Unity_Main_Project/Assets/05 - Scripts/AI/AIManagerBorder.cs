using System.Collections.Generic;
using UnityEngine;

public class AIManagerBorder : AIManager
{
    [Header("Border Game Objects")]
    [SerializeField] private Transform conveyor;
    [SerializeField] private Transform scanZone;
    [SerializeField] private Transform throwHere;
    [SerializeField] private Transform exitPoint;

   protected override void SpawnNPC()
   {
       if (spawnPoints == null || spawnPoints.Count == 0 || npcPrefab == null) return;
   
       Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
       AIController npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
   
       AIBorder borderAI = npc.GetComponent<AIBorder>();
       if (borderAI != null)
       {
           borderAI.Initialize(conveyor, scanZone, throwHere, exitPoint);
       }
   }

}
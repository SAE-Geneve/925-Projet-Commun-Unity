using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior; // Assure-toi d'avoir le bon namespace

public class AIManager : MonoBehaviour
{
    [Header("NPC Spawn Settings")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private float spawnInterval = 5f;

    private void Start()
    {
        if (spawnPoints.Count == 0 || npcPrefab == null)
        {
            Debug.LogWarning("AIManager: Aucun point de spawn ou prefab assigné !");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnNPC();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNPC()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        // 🔥 IMPORTANT : Cloner le Blackboard pour cette instance
        var behaviorGraphAgent = npc.GetComponent<BehaviorGraphAgent>();
        if (behaviorGraphAgent != null && behaviorGraphAgent.BlackboardReference != null)
        {
            // Crée une copie du Blackboard pour cet agent
            var newBlackboard = Instantiate(behaviorGraphAgent.BlackboardReference);
            behaviorGraphAgent.BlackboardReference = newBlackboard;
            
            Debug.Log($"NPC spawné avec Blackboard unique: {npc.name}");
        }
    }
}
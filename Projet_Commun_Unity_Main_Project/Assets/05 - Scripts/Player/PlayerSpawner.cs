using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] _spawnPoints;
    
    [Header("Parameters")]
    [SerializeField] private bool _spawnOnStart;
    [SerializeField] private bool _useSpawnRotation;


    private void Start()
    {
        if(_spawnOnStart) Spawn();

        
    }
    
    public void Spawn()
    {
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            StartCoroutine(WaitRagdollCoroutine(i));
        }
    }

    private IEnumerator WaitRagdollCoroutine(int idx)
    {
        List<PlayerController> players = PlayerManager.Instance.Players;
        yield return new WaitUntil(() => !players[idx].Ragdoll.IsRagdoll);
        Transform playerTransform = players[idx].transform;
        playerTransform.position = _spawnPoints[idx].position;
        if(_useSpawnRotation) playerTransform.rotation = _spawnPoints[idx].rotation;
    }
}
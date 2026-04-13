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

    private List<PlayerController> _players;

    private void Start()
    {
        if(_spawnOnStart) Spawn();

        _players = PlayerManager.Instance.Players;
    }
    
    public void Spawn()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            StartCoroutine(WaitRagdollCoroutine(i));
        }
    }

    private IEnumerator WaitRagdollCoroutine(int idx)
    {
        yield return new WaitUntil(() => !_players[idx].Ragdoll.IsRagdoll);
        Transform playerTransform = _players[idx].transform;
        playerTransform.position = _spawnPoints[idx].position;
        if(_useSpawnRotation) playerTransform.rotation = _spawnPoints[idx].rotation;
    }
}
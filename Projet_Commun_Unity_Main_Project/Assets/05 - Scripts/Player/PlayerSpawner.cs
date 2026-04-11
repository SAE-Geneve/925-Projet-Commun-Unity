using System.Collections;
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
            if (PlayerManager.Instance.Players[i].Ragdoll.IsRagdoll)
            {
                StartCoroutine(WaitRagdollCoroutine(i));
            }
            else
            {
                Transform playerTransform = PlayerManager.Instance.Players[i].transform;
                playerTransform.position = _spawnPoints[i].position;
                if(_useSpawnRotation) playerTransform.rotation = _spawnPoints[i].rotation;
            }
        }
    }

    private IEnumerator WaitRagdollCoroutine(int idx)
    {
        yield return new WaitUntil(() => !PlayerManager.Instance.Players[idx].Ragdoll.IsRagdoll);
        Transform playerTransform = PlayerManager.Instance.Players[idx].transform;
        playerTransform.position = _spawnPoints[idx].position;
        if(_useSpawnRotation) playerTransform.rotation = _spawnPoints[idx].rotation;
    }
}
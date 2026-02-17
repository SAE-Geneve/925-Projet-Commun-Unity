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
            Transform playerTransform = PlayerManager.Instance.Players[i].transform;
            playerTransform.position = _spawnPoints[i].position;
            if(_useSpawnRotation) playerTransform.rotation = _spawnPoints[i].rotation;
            
        }
    }
}
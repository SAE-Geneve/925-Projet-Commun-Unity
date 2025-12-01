using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] _spawnPoints;
    
    public void Spawn()
    {
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
            PlayerManager.Instance.Players[i].transform.position = _spawnPoints[i].position;
    }
}
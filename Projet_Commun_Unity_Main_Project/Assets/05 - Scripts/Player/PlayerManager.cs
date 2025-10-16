using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    private int _playerCount;
    
    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = spawnPoints[_playerCount].position;

        _playerCount++;
    }
    
    void OnPlayerLeft(PlayerInput player)
    {
        Debug.LogWarning("Player disconnected");
    }
}

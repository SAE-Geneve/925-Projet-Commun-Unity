using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private int _playerCount;
    
    private readonly List<PlayerController> _players = new ();
    
    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = spawnPoints[_playerCount].position;
        _players.Add(player.gameObject.GetComponent<PlayerController>());
        _playerCount++;
    }
    
    void OnPlayerLeft(PlayerInput player)
    {
        for (int i = 0; i < _playerCount; i++)
        {
            if (_players[i] == player.gameObject.GetComponent<PlayerController>())
            {
                _players[i] = null;
                _playerCount--;
                Debug.LogWarning("Player disconnected");
            }
        }
    }
}

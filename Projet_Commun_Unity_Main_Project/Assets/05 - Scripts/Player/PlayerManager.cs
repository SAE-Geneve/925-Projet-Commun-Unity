using System.Collections;
using System.Collections.Generic;
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
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        _players.Add(player.GetComponent<PlayerController>());
        _playerCount++;
    }
    
    private void OnPlayerDisconnect(PlayerInput player)
    {
        StartCoroutine(RemovePlayerNextFrame(player));
    }

    private IEnumerator RemovePlayerNextFrame(PlayerInput player)
    {
        player.user.UnpairDevices();

        yield return null; // wait one frame to let the Input System process internal cleanup

        for (int i = 0; i < _playerCount; i++)
        {
            if (_players[i] == player.GetComponent<PlayerController>())
            {
                Destroy(_players[i].gameObject);
                _players.RemoveAt(i);
                _playerCount--;
                Debug.LogWarning("Player disconnected");
                break;
            }
        }
    }
}

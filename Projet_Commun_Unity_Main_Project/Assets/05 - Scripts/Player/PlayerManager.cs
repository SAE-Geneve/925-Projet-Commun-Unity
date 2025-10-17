using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private int _playerCount;

    private readonly List<PlayerController> _players = new();

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = spawnPoints[_playerCount].position;
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        _players.Add(player.GetComponent<PlayerController>());
        _playerCount++;
    }

    private void OnPlayerDisconnect(PlayerInput player)
    {
        StartCoroutine(HandleDisconnectWithReconnectWindow(player, 5f)); // wait up to 5 seconds
    }

    private IEnumerator HandleDisconnectWithReconnectWindow(PlayerInput player, float reconnectTimeout)
    {
        var user = player.user;

        // Unpair immediately so input stops
        user.UnpairDevices();

        Debug.LogWarning($"Player {player.playerIndex + 1} controller disconnected — waiting for reconnection...");

        float timer = 0f;
        bool reconnected = false;

        // Subscribe temporarily to device connection events
        InputSystem.onDeviceChange += OnDeviceChange;

        void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Reconnected || change == InputDeviceChange.Added)
            {
                try
                {
                    // Try to pair the new device back to this player
                    user = InputUser.PerformPairingWithDevice(device, user);
                    reconnected = true;
                    Debug.Log($"Player {player.playerIndex + 1} controller reconnected.");
                }
                catch
                {
                    // Ignore invalid pairing attempts
                }
            }
        }

        // Wait for reconnection or timeout
        while (timer < reconnectTimeout && !reconnected)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Cleanup the listener
        InputSystem.onDeviceChange -= OnDeviceChange;

        if (!reconnected)
        {
            Debug.LogWarning(
                $"Player {player.playerIndex + 1} did not reconnect within {reconnectTimeout} seconds. Removing player.");
            yield return null; // give Input System a frame for cleanup
            RemovePlayer(player);
        }
        else
        {
            Debug.Log($"Player {player.playerIndex + 1} successfully reconnected!");
        }
    }

    private void RemovePlayer(PlayerInput player)
    {
        for (int i = 0; i < _playerCount; i++)
        {
            if (_players[i] == player.GetComponent<PlayerController>())
            {
                Destroy(_players[i].gameObject);
                _players.RemoveAt(i);
                _playerCount--;
                break;
            }
        }
    }
}
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
    public List<PlayerController> Players => _players;
    private GameManager _gameManager;
    private CameraManager _cameraManager;
    private PlayerInput _lastDisconnectPlayer;
    private bool _lastPlayerHasReconnected = false;


    private void Start()
    {
        _gameManager = GameManager.Instance;
        _cameraManager = FindFirstObjectByType<CameraManager>();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = spawnPoints[_playerCount].position;
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        _players.Add(player.GetComponent<PlayerController>());
        
        _playerCount++;
    }

    private void OnPlayerDisconnect(PlayerInput player)
    {
        _gameManager.OnPlayerDisconnected();
        
        if (_gameManager.State == GameState.Disconnected)
        {
            _lastDisconnectPlayer = player;
            StartCoroutine(HandleDisconnectWithReconnectWindow()); 
        }
    }
    
    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change is not (InputDeviceChange.Reconnected or InputDeviceChange.Added)) return;
        try
        {
            // Try to pair the new device back to this player
            InputUser.PerformPairingWithDevice(device, _lastDisconnectPlayer.user);
            _lastPlayerHasReconnected = true;
            Debug.Log($"Player {_lastDisconnectPlayer.playerIndex + 1} controller reconnected.");
        }
        catch
        {
            // Ignore invalid pairing attempts
        }
    }

    private IEnumerator HandleDisconnectWithReconnectWindow()
    {
        // Unpair immediately so input stops
        _lastDisconnectPlayer.user.UnpairDevices();

        Debug.LogWarning($"Player {_lastDisconnectPlayer.playerIndex + 1} controller disconnected — waiting for reconnection...");

        // Subscribe temporarily to device connection events
        InputSystem.onDeviceChange += OnDeviceChange;

        // Wait for reconnection or timeout
        while (!_lastPlayerHasReconnected)
        {
            yield return null;
        }
        
        ClearLastPlayer();
        _lastPlayerHasReconnected = false;
        
        _gameManager.OnPlayerReconnected();
    }

    private void ClearLastPlayer()
    {
        // Cleanup the listener
        InputSystem.onDeviceChange -= OnDeviceChange;

        _lastDisconnectPlayer = null;
    }

    public void OnReconnectionTimeOut()
    {
        StopAllCoroutines();
        
        RemovePlayer(_lastDisconnectPlayer);
        ClearLastPlayer();
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
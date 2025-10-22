using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] spawnPoints;
    public List<PlayerController> Players => _players;
    
    private readonly List<PlayerController> _players = new();
    private PlayerController _lastDisconnectPlayer;
    
    private GameManager _gameManager;
    public int PlayerCount => _players.Count;
    public static PlayerManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    private void Start() => _gameManager = GameManager.Instance;

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = spawnPoints[_players.Count].position;
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        _players.Add(player.GetComponent<PlayerController>());
    }

    private void OnPlayerDisconnect(PlayerController player)
    {
        _gameManager.OnPlayerDisconnected();
        
        if (_gameManager.State != GameState.Disconnected) return;
        
        _lastDisconnectPlayer = player;
        _lastDisconnectPlayer.Input.user.UnpairDevices();
        
        InputSystem.onDeviceChange += OnDeviceChange;
        
        Debug.LogWarning($"Player {_lastDisconnectPlayer.Input.playerIndex + 1} controller disconnected — waiting for reconnection...");
    }
    
    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change is not (InputDeviceChange.Reconnected or InputDeviceChange.Added)) return;
        try
        {
            InputUser.PerformPairingWithDevice(device, _lastDisconnectPlayer.Input.user);
            OnPlayerReconnect();
            Debug.Log($"Player {_lastDisconnectPlayer.Input.playerIndex + 1} controller reconnected.");
        }
        catch
        {
            // Ignore invalid pairing attempts
        }
    }

    private void CleanupDisconnectedPlayer()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        _lastDisconnectPlayer = null;
    }

    private void OnPlayerReconnect()
    {
        CleanupDisconnectedPlayer();
        _gameManager.OnPlayerReconnected();
    }

    public void OnReconnectionTimeOut()
    {
        RemovePlayer(_lastDisconnectPlayer);
        CleanupDisconnectedPlayer();
    }

    private void RemovePlayer(PlayerController player)
    {
        
        if (_players.Contains(player))
        {
            Destroy(player.gameObject);
           _players.Remove(player);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public List<PlayerController> Players => _players;
    
    public event Action<PlayerController> OnPlayerConnected;
    public event Action<PlayerController> OnReconnectTimerOut;
    public event Action OnPlayerRemoved;
    public int PlayerCount => _players.Count;
    
    private readonly List<PlayerController> _players = new();
    private PlayerController _lastDisconnectPlayer;
    
    private GameManager _gameManager;
    public static PlayerManager Instance { get; private set; }
    
    [SerializeField] private Transform trackingTarget;

    public Transform TrackingTarget => trackingTarget;

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;

        SceneManager.sceneLoaded += SetPlayerToSpawnPoint;
    }
    
    private void Start() => _gameManager = GameManager.Instance;

    private void SetPlayerToSpawnPoint(Scene scene, LoadSceneMode mode)
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");

        for (int i = 0; i < PlayerCount; i++)
        {
            _players[i].transform.position = spawnPoints[i].transform.position;
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = Vector3.zero;/*Add a spawn point*/
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        _players.Add(player.GetComponent<PlayerController>());
        OnPlayerConnected?.Invoke(player.GetComponent<PlayerController>());
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
        OnReconnectTimerOut?.Invoke(_lastDisconnectPlayer);
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
        
        OnPlayerRemoved?.Invoke();
    }
}
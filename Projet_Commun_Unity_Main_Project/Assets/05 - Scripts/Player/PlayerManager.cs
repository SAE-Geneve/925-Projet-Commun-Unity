using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform trackingTarget;
    public event Action<PlayerController> OnPlayerConnected;
    public event Action<PlayerController> OnReconnectTimerOut;
    public event Action OnPlayerAdded;
    public event Action OnPlayerRemoved;
    
    public List<PlayerController> Players => _players;
    public int PlayerCount => _players.Count;
    
    private readonly List<PlayerController> _players = new();
    private PlayerController _lastDisconnectPlayer;
    
    private GameManager _gameManager;
    public static PlayerManager Instance { get; private set; }

    private PlayerInputManager _playerInputManager;
    
    private GameObject[] _spawnPoints;
    
    public PlayerInputManager PlayerInputManager => _playerInputManager;

    private bool _arePlayersActive;

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
        
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        _spawnPoints = _spawnPoints.OrderBy(x => x.name).ToArray();
    }

    public void EnablePlayerControllers()
    {
        _arePlayersActive = true;
        
        foreach (var player in _players)
        {
            player.InputManager.active = true;
            // player.GetComponentInChildren<ParticleSystem>().Play();
            // player.Display.ShowHalo(true);
        }
    }

    public void Reset()
    {
        foreach (var pl in _players)
            Destroy(pl.gameObject);
        
        _players.Clear();
    }
    
    public void DisablePlayerControllers()
    {
        _arePlayersActive = false;
        foreach (var player in _players)
        {
            player.InputManager.active = false;
            // player.GetComponentInChildren<ParticleSystem>().Stop();
            // player.Display.ShowHalo(false);
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = _spawnPoints[Players.Count].transform.position;
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        
        _players.Add(player.GetComponent<PlayerController>());
        // SetPlayerColors(player.GetComponentInChildren<ParticleSystem>());

        CharacterDisplay display = player.GetComponent<CharacterDisplay>();
        
        SetOutline(display.SkinnedMeshRenderer);

        if (!_arePlayersActive)
        {
            player.GetComponent<InputManager>().active = false;
            // player.GetComponentInChildren<ParticleSystem>().Stop();
            // display.ShowHalo(false);
        }
        
        OnPlayerAdded?.Invoke();
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
    
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var player in _players)
            player.Movement.SetupCamera();
    }

    private void SetOutline(SkinnedMeshRenderer skinnedMeshRenderer)
    {
        Debug.Log(skinnedMeshRenderer);
        switch (_players.Count)
        {
            case 0 : skinnedMeshRenderer.renderingLayerMask &= ~RenderingLayerMask.GetMask("Outline1"); break;
            case 1 : skinnedMeshRenderer.renderingLayerMask &= ~RenderingLayerMask.GetMask("Outline2"); break;
            case 2 : skinnedMeshRenderer.renderingLayerMask &= ~RenderingLayerMask.GetMask("Outline3"); break;
            case 3 : skinnedMeshRenderer.renderingLayerMask &= ~RenderingLayerMask.GetMask("Outline4"); break;
        }
    }
}
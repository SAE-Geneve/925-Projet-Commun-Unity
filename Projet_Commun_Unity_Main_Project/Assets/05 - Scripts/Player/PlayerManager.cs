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
    
    [Header("Parameters")]
    [SerializeField] private Color _color1 = Color.red;
    [SerializeField] private Color _color2 = Color.blue;
    [SerializeField] private Color _color3 = Color.green;
    [SerializeField] private Color _color4 = Color.yellow;
    
    public List<PlayerController> Players => _players;
    
    public event Action<PlayerController> OnPlayerConnected;
    public event Action<PlayerController> OnReconnectTimerOut;
    public event Action OnPlayerAdded;
    public event Action OnPlayerRemoved;
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
            player.Display.ShowHalo(true);
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
            player.Display.ShowHalo(false);
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = _spawnPoints[Players.Count].transform.position;
        player.GetComponent<InputManager>().OnControllerDisconnected += OnPlayerDisconnect;
        
        _players.Add(player.GetComponent<PlayerController>());
        // SetPlayerColors(player.GetComponentInChildren<ParticleSystem>());

        CharacterDisplay display = player.GetComponent<CharacterDisplay>();
        
        SetHaloColors(display.Halo);

        if (!_arePlayersActive)
        {
            player.GetComponent<InputManager>().active = false;
            // player.GetComponentInChildren<ParticleSystem>().Stop();
            display.ShowHalo(false);
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

    private void SetPlayerColors(ParticleSystem particleSystem)
    {
        var colt = particleSystem.colorOverLifetime;
        Gradient grad = new Gradient
        {
            mode = GradientMode.Fixed
        };

        var colors = new GradientColorKey[2];
        
        switch (_players.Count)
        {
            case 1:
                colors[0] = new GradientColorKey(new Color(255,0,0,1), 0.5f);
                colors[1] = new GradientColorKey(new Color(128,0,0,1), 1.0f);
                break;            
            case 2:
                colors[0] = new GradientColorKey(new Color(0,255,0,1), 0.5f);
                colors[1] = new GradientColorKey(new Color(0,128,0,1), 1.0f);
                break;            
            case 3:
                colors[0] = new GradientColorKey(new Color(0,0,255,1), 0.5f);
                colors[1] = new GradientColorKey(new Color(0,0,128,1), 1.0f);
                break;            
            case 4:
                colors[0] = new GradientColorKey(new Color(255,255,0,1), 0.5f);
                colors[1] = new GradientColorKey(new Color(128,128,0,1), 1.0f);
                break;            
            default:
                colors[0] = new GradientColorKey(new Color(0,0,0,1), 0.5f);
                colors[1] = new GradientColorKey(new Color(128,128,128,1), 1.0f);
                break;
        }
        
        var alphas = new GradientAlphaKey[1];
        alphas[0] = new GradientAlphaKey(1, 0);
        grad.SetKeys(colors, alphas);
        
        colt.color = grad;
    }

    private void SetHaloColors(Image halo)
    {
        switch (_players.Count)
        {
            case 1 : halo.color = _color1; break;
            case 2 : halo.color = _color2; break;
            case 3 : halo.color = _color3; break;
            case 4 : halo.color = _color4; break;
        }
    }
}
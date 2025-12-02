using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The missions that will be played in order")] 
    [SerializeField] private Mission[] _missions;

    [Header("Parameters")] [Tooltip("The global timer initial time")] [SerializeField] [Min(0)]
    private float _initialTimer = 90f;

    [Tooltip("The initial time to wait when a player is disconnected")] [SerializeField] [Min(0)]
    private float _initialDisconnectionTime = 60f;

    [Tooltip("The maximal client satisfaction point we can get")] [SerializeField] [Min(0)]
    private int _maxSatisfaction = 100;

    [Tooltip("The minimum numbers of players needed to start the game")] [SerializeField] [UnityEngine.Range(1,4)]
    private int _minPlayers = 1;

    public static GameManager Instance { get; private set; }

    // Events
    public event Action OnTimerUpdate;
    public event Action OnDisconnectionTimerUpdate;

    // Getter/Setter
    public float Timer
    {
        get => _timer;
        private set
        {
            if (value <= 0)
            {
                _timer = 0;
                Debug.Log("Game ended! Back to menu for the moment");
                MenuReset();
            }
            else _timer = value;

            OnTimerUpdate?.Invoke();
        }
    }

    public float DisconnectionTimer
    {
        get => _disconnectionTimer;
        private set
        {
            if (value <= 0) ReconnectionTimeOut();
            else _disconnectionTimer = value;

            OnDisconnectionTimerUpdate?.Invoke();
        }
    }

    public Mission CurrentMission { get; private set; }

    private PlayerManager _playerManager;

    private Dictionary<MissionID, Mission> _missionMap;

    public GameState State => _state;
    public GameContext Context => _context;


    public int ClientSatisfaction
    {
        get => _clientSatisfaction;
        set => _clientSatisfaction = Mathf.Clamp(value, 0, _maxSatisfaction);
    }

    // Members
    private GameState _state;
    private GameState _lastState;
    private GameContext _context;

    //private Mission _currentMission;

    private float _timer;
    private float _disconnectionTimer;

    private int _clientSatisfaction;

    #region Initialization

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        if(_playerManager) _playerManager.PlayerInputManager.DisableJoining();
        ResetGame();
        Debug.Log($"Game State: {_state}");
    }

    #endregion

    private void Update()
    {
        if (_state == GameState.Playing)
            Timer -= Time.deltaTime;
        else if (_state == GameState.Disconnected)
            DisconnectionTimer -= Time.deltaTime;
    }

    public void SwitchState(GameState newState)
    {
        if (newState == _state)
        {
            Debug.LogWarning($"Already in {_state} state, cannot change");
            return;
        }

        if (_playerManager)
        {
            if (newState == GameState.Lobby || _context == GameContext.Hub)
            {
                _playerManager.PlayerInputManager.EnableJoining();
            }
            else
            {
                _playerManager.PlayerInputManager.DisableJoining();
            }

            if (newState == GameState.Lobby || newState == GameState.Menu || newState == GameState.Paused)
            {
                _playerManager.DisablePlayerControllers();
            }
            else
            {
                _playerManager.EnablePlayerControllers();
            }
        }

        _state = newState;
        Debug.Log($"Game State: {_state}");
    }

    #region Reset

    public void MenuReset()
    {
        if(_playerManager) _playerManager.Reset();
        SwitchState(GameState.Menu);
        ResetGame();
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("MainMenu");
        Destroy(transform.parent.gameObject);
    }

    public void ResetGame()
    {
        Timer = _initialTimer;
        DisconnectionTimer = _initialDisconnectionTime;

        BuildMissionMap();

        _context = GameContext.Hub;
    }

    #endregion

    public void StartGame()
    {
        if (_playerManager.Players.Count < _minPlayers) return;

        SwitchState(GameState.Playing);
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        SceneLoader.Instance.LoadScene("HubScene");
    }

    public void StartCinematic()
    {
        if (_state != GameState.Playing)
            Debug.LogWarning("Can only start cinematic when the game is in playing state");
        else SwitchState(GameState.Cinematic);
    }

    #region Mission

    private void BuildMissionMap()
    {
        _missionMap = new Dictionary<MissionID, Mission>();
        
        if(_missions == null || _missions.Length == 0) return;

        foreach (Mission mission in _missions)
        {
            if (!mission) continue;

            if (!_missionMap.TryAdd(mission.ID, mission))
                Debug.LogWarning($"Can't add {mission.ID} mission");
        }
    }

    public void StartMission(Mission mission)
    {
        if (_state != GameState.Playing || _context != GameContext.Hub)
        {
            Debug.LogWarning("Mission can only be started when game is playing in the hub");
            return;
        }

        CurrentMission = mission;
        if(_playerManager) _playerManager.PlayerInputManager.DisableJoining();
        _context = GameContext.Mission;
    }

    public void StopMission()
    {
        if (_state != GameState.Playing || _context == GameContext.Hub)
        {
            Debug.LogWarning("Mission can only be stopped when game is playing in a mission");
            return;
        }

        CurrentMission = null;
        if(_playerManager) _playerManager.PlayerInputManager.EnableJoining();
        _context = GameContext.Hub;
    }

    public Mission GetMission(MissionID id)
    {
        if (_missionMap.TryGetValue(id, out Mission mission))
            return mission;

        Debug.LogWarning($"MissionID {id} not found");
        return null;
    }

    #endregion

    #region Pause

    /// <summary>
    /// Triggered when the pause input is performed, it will resume the game
    /// if it's already in pause state
    /// </summary>
    public void PauseTrigger()
    {
        if (_state == GameState.Paused)
            Unpause();
        else if (_state == GameState.Playing || _state == GameState.Cinematic)
            Pause();
        else Debug.LogWarning("Can only pause when playing or cinematic");
    }

    private void Pause()
    {
        _lastState = _state;
        SwitchState(GameState.Paused);
        
        Time.timeScale = 0;
        
        // if(_playerManager)
        //     foreach (var pl in _playerManager.Players)
        //         pl.InputManager.active = false;

        if(UIManager.Instance) UIManager.Instance.ShowPauseCanvas(true);
        Debug.Log($"Game paused (from {_lastState})");
    }

    private void Unpause()
    {
        SwitchState(_lastState);
        
        Time.timeScale = 1f;
        
        // if(_playerManager)
        //     foreach (var pl in _playerManager.Players)
        //         pl.InputManager.active = true;

        if(UIManager.Instance) UIManager.Instance.ShowPauseCanvas(false);
        Debug.Log($"Game unpaused (back to {_state})");
    }

    #endregion

    #region Disconnection

    /// <summary>
    /// Triggered when a player is disconnected
    /// </summary>
    public void OnPlayerDisconnected()
    {
        if (_state != GameState.Playing && _state != GameState.Cinematic)
        {
            Debug.LogWarning("Can only start the disconnection timer when the game is in playing/cinematic state");
            return;
        }

        _lastState = _state;
        SwitchState(GameState.Disconnected);
        // TODO: Handle when a player is disconnected

        Debug.Log("One or multiple players have been disconnected");
    }

    /// <summary>
    /// All disconnected players reconnected before the timer is finished
    /// </summary>
    public void OnPlayerReconnected()
    {
        if (_state != GameState.Disconnected)
        {
            Debug.LogWarning("Can only reconnect when the game is in disconnected state");
            return;
        }

        // TODO: Handle when the disconnected player reconnected
        SwitchState(_lastState);
        DisconnectionTimer = _initialDisconnectionTime;

        Debug.Log("All players have been reconnected");
    }
    
    private void ReconnectionTimeOut()
    {
        _disconnectionTimer = _initialDisconnectionTime;
        _playerManager.OnReconnectionTimeOut();
        
        if (_playerManager.Players.Count < _minPlayers) MenuReset();
    }

    #endregion
}

public enum GameState
{
    Menu,
    Lobby,
    Playing,
    Paused,
    Cinematic,
    Disconnected
}

public enum GameContext
{
    Hub,
    Mission,
    LastMission,
}
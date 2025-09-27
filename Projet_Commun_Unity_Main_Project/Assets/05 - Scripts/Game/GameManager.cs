using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")] [SerializeField] [Min(0)]
    private float _initialTimer = 30f;

    public static GameManager Instance { get; private set; }

    public float Timer { get; private set; }

    private GameState _state;
    private GameState _lastState;
    private GameContext _context;

    #region Initialization

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        Timer = _initialTimer;
        
        _state = GameState.Menu;
        _context = GameContext.Hub;
        
        Debug.Log($"Game State: {_state}");
    }

    #endregion

    private void Update()
    {
        if (_state == GameState.Playing)
            Timer -= Time.deltaTime;
        
        //Debug.Log($"Timer: {Timer}");
    }

    public void SwitchState(GameState newState)
    {
        if (newState == _state)
        {
            Debug.LogWarning($"Already in {_state} state, cannot change");
            return;
        }
        
        _state = newState;
        Debug.Log($"Game State: {_state}");
    }

    public void StartGame()
    {
        SwitchState(GameState.Playing);
        
        // TODO: Spawn players in the Hub
    }
    
    public void StartMission()
    {
        if (_state != GameState.Playing && _context != GameContext.Hub)
        {
            Debug.LogWarning("Mission can only be started when game is playing in the hub");
            return;
        }

        _context = GameContext.Mission;
    }

    #region Pause

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
        _state = GameState.Paused;
        Debug.Log($"Game paused (from {_lastState})");
    }

    private void Unpause()
    {
        _state = _lastState;
        Debug.Log($"Game unpaused (back to {_state})");
    }

    #endregion
    
    #region Deconnexion

    public void OnPlayerDisconnected()
    {
        SwitchState(GameState.Disconnected);
        // TODO: Handle when a player is disconnected
    }

    public void OnPlayerReconnected()
    {
        // TODO: Handle when the disconnected player reconnected
        SwitchState(GameState.Playing);
    }

    public void OnDisconnectedTimeOut()
    {
        SwitchState(GameState.Menu); // Maybe make the scene transition and after that switch the state into "Menu"
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
    LastMission
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")] [SerializeField] [Min(0)]
    private float _initialTimer = 30f;

    public static GameManager Instance { get; private set; }

    public float Timer { get; private set; }

    private GameState _gameState;

    #region Initialization

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        Timer = _initialTimer;
        _gameState = GameState.Playing;
    }

    #endregion

    private void Update()
    {
        if (_gameState == GameState.Playing)
            Timer -= Time.deltaTime;
        
        //Debug.Log($"Timer: {Timer}");
    }

    public void StartMission()
    {
        
    }

    public void PauseTrigger()
    {
        if (_gameState != GameState.Playing || _gameState == GameState.Cinematic) return;

        // TODO: Display pause panel
    }

    #region Deconnexion

    public void OnPlayerDisconnected()
    {
        // TODO: Handle when a player is disconnected
    }

    public void OnPlayerReconnected()
    {
        // TODO: Handle when the disconnected player reconnected
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
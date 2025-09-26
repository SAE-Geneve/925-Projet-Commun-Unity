using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] [Min(0)] private float _initialTimer = 30f;
    
    public static GameManager Instance { get; private set; }

    public enum State
    {
        Running,
        Paused,
        Cinematic,
        Disconnected
    }
    
    public float Timer { get; private set; }

    private State _state;
    
    #region Initialization 

    private void Awake()
    {
        if(Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        Timer = _initialTimer;
        _state = State.Running;
    }

    #endregion

    private void Update()
    {
        switch (_state)
        {
            case State.Running: Timer -= Time.deltaTime; break;
            case State.Paused: break;
            case State.Cinematic: break;
            case State.Disconnected: break;
        }
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
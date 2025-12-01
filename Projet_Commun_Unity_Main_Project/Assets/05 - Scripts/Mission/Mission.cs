using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] _missionSpawnPoints;
    
    [Header("Parameters")] 
    [SerializeField] private string _name = "New Mission";
    [SerializeField] private MissionID _missionID = MissionID.BorderControl;
    [SerializeField] protected MissionState _missionState = MissionState.Unlocked;

    [Header("Timer")] 
    [SerializeField] [Min(1f)] private float _initialTimer = 60f;
    [SerializeField] private bool _isTimerActive;

    [Header("Events")] 
    [SerializeField] private UnityEvent _onMissionStarted;
    [SerializeField] private UnityEvent _onMissionFinished;
    
    public MissionID ID => _missionID;
    public bool IsLocked => _missionState == MissionState.Locked;
    
    public float Timer
    {
        get => _timer;
        private set
        {
            if (value < 0) Finish();
            _timer = value;
        }
    }
    
    protected enum MissionState
    {
        Unlocked,
        Locked,
        Playing,
        Finished
    }

    private float _timer;
    
    private GameManager _gameManager;
    private UIManager _uiManager;

    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;
        
        SwitchMissionState(_missionState);
        if(_missionState == MissionState.Unlocked) UpdateTargetMission();
    }

    private void Update()
    {
        if(_isTimerActive 
           && _gameManager.State == GameState.Playing 
           && _missionState == MissionState.Playing)
            Timer -= Time.deltaTime;
    }

    public void StartMission()
    {
        if (_missionState == MissionState.Locked) Debug.LogWarning("Mission locked, cannot start mission");
        else OnStartMission();
    }

    private void OnStartMission()
    {
        Timer = _initialTimer;
        
        _gameManager.StartMission(this);
        
        SwitchMissionState(MissionState.Playing);
        
        _onMissionStarted?.Invoke();

        Debug.Log($"Mission {_name} began");
    }
    
    public void Finish()
    {
        GameManager.Instance.StopMission();
        
        SwitchMissionState(MissionState.Finished);
        _onMissionFinished?.Invoke();
    
        Debug.Log($"Mission {_name} finished");
    }

    public void Unlock()
    {
        SwitchMissionState(MissionState.Unlocked);
        UpdateTargetMission();
    }

    protected virtual void SwitchMissionState(MissionState newState)
    {
        if(newState != _missionState) _missionState = newState;
    }

    private void UpdateTargetMission() => _uiManager.TargetMission = _missionID;

    public void SpawnPlayers()
    {
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
            PlayerManager.Instance.Players[i].transform.position = _missionSpawnPoints[i].position;
    }
}

public enum MissionID
{
    BorderControl,
    ConveyorBelt,
    Boarding,
    LostLuggage,
    Plane
}
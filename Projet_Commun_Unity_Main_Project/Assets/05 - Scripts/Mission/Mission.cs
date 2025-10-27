using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
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
    
    public float Timer
    {
        get => _timer;
        private set
        {
            if (_timer < 0) Finish();
            _timer = value;
        }
    }

    private float _timer;
    
    protected enum MissionState
    {
        Unlocked,
        Locked,
        Playing,
        Finished
    }
    
    private GameManager _gameManager;

    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        
        SwitchMissionState(_missionState);
        
        Timer = _initialTimer;
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
        SceneLoader.Instance.LoadScene(_name);
        
        _gameManager.StartMission(this);
        
        SwitchMissionState(MissionState.Playing);
        
        _onMissionStarted?.Invoke();
        Debug.Log($"Mission {_name} began");
    }
    
    protected void Finish()
    {
        _missionState = MissionState.Finished;
        
        _gameManager.StopMission();
        
        SwitchMissionState(MissionState.Finished);
        
        _onMissionFinished?.Invoke();
        
        Debug.Log($"Mission {_name} finished");
    }
    
    public void Unlock() => SwitchMissionState(MissionState.Unlocked);

    protected virtual void SwitchMissionState(MissionState newState)
    {
        if(newState != _missionState) _missionState = newState;
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
using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private string _name = "New Mission";
    [SerializeField] private MissionID _missionID = MissionID.BorderControl;
    [SerializeField] protected MissionState _missionState = MissionState.Unlocked;

    [Header("Events")] 
    [SerializeField] private UnityEvent _onMissionStarted;
    [SerializeField] private UnityEvent _onMissionFinished;
    
    public MissionID ID => _missionID;
    
    protected enum MissionState
    {
        Unlocked,
        Locked,
        Playing,
        Finished
    }
    
    protected GameManager _gameManager;

    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        
        SwitchMissionState(_missionState);
    }

    public void StartMission()
    {
        if (_missionState == MissionState.Locked) Debug.LogWarning("Mission locked, cannot start mission");
        else OnStartMission();
    }

    protected virtual void OnStartMission()
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
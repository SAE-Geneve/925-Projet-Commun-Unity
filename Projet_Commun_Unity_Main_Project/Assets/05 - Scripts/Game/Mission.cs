using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private string _name = "New Mission";
    [SerializeField] protected MissionState _missionState = MissionState.Unlocked;

    [Header("Events")] 
    [SerializeField] private UnityEvent _onMissionFinished;
    
    protected enum MissionState
    {
        Unlocked,
        Locked,
        Playing,
        Finished
    }
    
    GameManager _gameManager;
    
    private bool _missionPlaying;

    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        
        SwitchMissionState(_missionState);
    }

    public void StartMission()
    {
        if (_missionState == MissionState.Locked)
        {
            Debug.LogWarning("Mission locked, cannot start mission");
            return;
        }
        
        _gameManager.StartMission();
        
        SwitchMissionState(MissionState.Playing);
        
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
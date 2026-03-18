using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private string _name = "New Mission";
    [SerializeField] protected MissionState _missionState = MissionState.Unlocked;

    [Header("UI")]
    [SerializeField] private GameObject _explanationPrefab; 
    public GameObject ExplanationPrefab => _explanationPrefab;
    
    [Header("Timer")] 
    [SerializeField] [Min(1f)] private float _initialTimer = 60f;
    [SerializeField] private bool _isTimerActive;

    [Header("Events")] 
    [SerializeField] private UnityEvent _onMissionStarted;
    [SerializeField] private UnityEvent _onMissionFinished;
    
    
    
    private enum MissionName
    {
        BorderControl,
        Boarding,
        ConvoyerBelt,
        LostLuggage,
    }

    [SerializeField] private MissionName _missionName;
    private AudioManager _audioManager;
    public event Action OnSwitchState;
    
    public string Name => _name;
    public bool IsLocked => _missionState == MissionState.Locked;
    
    public float Timer
    {
        get => _timer;
        private set
        {
            if (value < 0) Finish(true);
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
        _audioManager = AudioManager.Instance;
        SwitchMissionState(_missionState);
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
        _audioManager.StopBGM();
        switch (_missionName)
        {
            case MissionName.BorderControl:
                _audioManager.PlayBGM(_audioManager.borderControleMusic);
                break;
            case MissionName.Boarding:
                _audioManager.PlayBGM(_audioManager.bordingMusic);
                break;
            case MissionName.ConvoyerBelt:
                _audioManager.PlayBGM(_audioManager.conveyorBeltMusic);
                break;
            case MissionName.LostLuggage:
                _audioManager.PlayBGM(_audioManager.lostLuggageMusic);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Timer = _initialTimer;
        _gameManager.StartMission(this);
        SwitchMissionState(MissionState.Playing);
        _onMissionStarted?.Invoke();
        Debug.Log($"Mission {_name} began");
    }
    
    public void Finish(bool victory)
    {
        _audioManager.StopBGM();
        _audioManager.PlayBGM(_audioManager.hubMusic);
        if (victory)
            _gameManager.Timer += _initialTimer / 2f;

        _gameManager.StopMission(false);
        SwitchMissionState(MissionState.Finished);
        _onMissionFinished?.Invoke();
        // TimelineManager.Instance?.PlayResult(victory);
        Debug.Log($"Mission {_name} finished");
    }

    public void Unlock() => SwitchMissionState(MissionState.Unlocked);
    public void Lock() => SwitchMissionState(MissionState.Locked);

    protected virtual void SwitchMissionState(MissionState newState)
    {
        if (newState != _missionState)
        {
            _missionState = newState;
            OnSwitchState?.Invoke();
        }
    }
}